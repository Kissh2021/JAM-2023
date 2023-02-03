using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Generics
{
	public static class CurveTools
	{
		#region Bezier

		public static Vector3[] SplineBezier(Vector3[] controlPoints, int count)
		{
			Vector3[] toReturn = new Vector3[count];
			Vector3 pos = Vector3.zero;

			for (int i = 0; i < count; ++i)
			{
				float t = (float) i / (count - 1);

				pos = Vector3.zero;
				for (int k = 0; k < controlPoints.Length; k++)
				{
					pos += Bernstein(controlPoints.Length - 1, k, t) * controlPoints[k];
				}

				toReturn[i] = pos;
			}

			return toReturn;
		}

		private static float Bernstein(int n, int k, float t) =>
			Combination(k, n) * IntPow(t, k) * IntPow(1 - t, n - k);

		private static int Combination(int k, int n) => Factorial(n) / (Factorial(k) * Factorial(n - k));

		private static int Factorial(int n) => Enumerable.Range(1, n).Aggregate(1, (p, item) => p * item);

		private static float IntPow(float number, int pow)
		{
			if (pow == 0) return 1;
			if (pow == 1) return number;

			float result = number;
			for (int i = 1; i < pow; ++i)
				result *= number;

			return result;
		}

		#endregion

		#region Cubic

		//TODO : manage to make it work with Vector3
		public static Vector2[] SplineCubic(Vector2[] points, int count, int start = 0, int end = int.MaxValue)
		{
			if (points is null)
				throw new ArgumentException($"{nameof(points)} must not be null");
			if (points.Length <= 1)
				throw new ArgumentException($"{nameof(points)} has too few elements");
			if (start < 0 || start >= points.Length)
				throw new ArgumentException($"{nameof(start)} must be in [0, {points.Length - 1}]");
			if (end <= 0 || end != int.MaxValue && end >= points.Length)
				throw new ArgumentException($"{nameof(end)} must be in [1, {points.Length - 1}]");

			if (end == int.MaxValue)
				end = points.Length - 1;

			double[] xs = new double[points.Length];
			double[] ys = new double[points.Length];
			for (int i = 0; i < points.Length; i++)
			{
				xs[i] = points[i].x;
				ys[i] = points[i].y;
			}

			var interpolatedPos = InterpolateXY(xs, ys, count);
			List<Vector2> toReturn = new List<Vector2>();
			bool hasPastStart = false;
			bool hasPastEnd = false;
			int endPointPassCount = 0;
			for (int i = start; i <= end; i++)
			{
				if (points[i] == points[end])
				{
					endPointPassCount++;
				}
			}

			Vector2 point = new Vector2();
			for (int i = 0; i < interpolatedPos.xs.Length && !hasPastEnd; i++)
			{
				point.x = (float) interpolatedPos.xs[i];
				point.y = (float) interpolatedPos.ys[i];

				//TODO : régler le problème de spline qui boucle pas bien/pas tout court
				if (hasPastStart && (points[end] - point).sqrMagnitude < 0.05f / count)
					endPointPassCount--;
				hasPastEnd = endPointPassCount == 0;
				hasPastStart = hasPastStart || (points[start] - point).sqrMagnitude < 0.05f;

				if (hasPastStart) toReturn.Add(point);
			}

			// Vector2[] rawData = new Vector2[interpolatedPos.xs.Length];
			// for (int i = 0; i < rawData.Length; i++)
			// {
			// 	rawData[i].x = (float) interpolatedPos.xs[i];
			// 	rawData[i].y = (float) interpolatedPos.ys[i];
			// }

			return toReturn.ToArray();
		}

		/// <summary>
		/// Generate a smooth (interpolated) curve that follows the path of the given X/Y points
		/// </summary>
		private static (double[] xs, double[] ys) InterpolateXY(double[] xs, double[] ys, int count)
		{
			if (xs is null || ys is null || xs.Length != ys.Length)
				throw new ArgumentException($"{nameof(xs)} and {nameof(ys)} must have same length");

			int inputPointCount = xs.Length;
			double[] inputDistances = new double[inputPointCount];
			for (int i = 1; i < inputPointCount; i++)
			{
				double dx = xs[i] - xs[i - 1];
				double dy = ys[i] - ys[i - 1];
				double distance = Math.Sqrt(dx * dx + dy * dy);
				inputDistances[i] = inputDistances[i - 1] + distance;
			}

			double meanDistance = inputDistances.Last() / (count - 1);
			double[] evenDistances = Enumerable.Range(0, count).Select(x => x * meanDistance).ToArray();
			double[] xsOut = Interpolate(inputDistances, xs, evenDistances);
			double[] ysOut = Interpolate(inputDistances, ys, evenDistances);
			return (xsOut, ysOut);
		}

		private static double[] Interpolate(double[] xOrig, double[] yOrig, double[] xInterp)
		{
			(double[] a, double[] b) = FitMatrix(xOrig, yOrig);

			double[] yInterp = new double[xInterp.Length];
			for (int i = 0; i < yInterp.Length; i++)
			{
				int j;
				for (j = 0; j < xOrig.Length - 2; j++)
					if (xInterp[i] <= xOrig[j + 1])
						break;

				double dx = xOrig[j + 1] - xOrig[j];
				double t = (xInterp[i] - xOrig[j]) / dx;
				double y = (1 - t) * yOrig[j] + t * yOrig[j + 1] +
				           t * (1 - t) * (a[j] * (1 - t) + b[j] * t);
				yInterp[i] = y;
			}

			return yInterp;
		}

		private static (double[] a, double[] b) FitMatrix(double[] x, double[] y)
		{
			int n = x.Length;
			double[] a = new double[n - 1];
			double[] b = new double[n - 1];
			double[] r = new double[n];
			double[] A = new double[n];
			double[] B = new double[n];
			double[] C = new double[n];

			double dx1, dx2, dy1, dy2;

			dx1 = x[1] - x[0];
			C[0] = 1.0f / dx1;
			B[0] = 2.0f * C[0];
			r[0] = 3 * (y[1] - y[0]) / (dx1 * dx1);

			for (int i = 1; i < n - 1; i++)
			{
				dx1 = x[i] - x[i - 1];
				dx2 = x[i + 1] - x[i];
				A[i] = 1.0f / dx1;
				C[i] = 1.0f / dx2;
				B[i] = 2.0f * (A[i] + C[i]);
				dy1 = y[i] - y[i - 1];
				dy2 = y[i + 1] - y[i];
				r[i] = 3 * (dy1 / (dx1 * dx1) + dy2 / (dx2 * dx2));
			}

			dx1 = x[n - 1] - x[n - 2];
			dy1 = y[n - 1] - y[n - 2];
			A[n - 1] = 1.0f / dx1;
			B[n - 1] = 2.0f * A[n - 1];
			r[n - 1] = 3 * (dy1 / (dx1 * dx1));

			double[] cPrime = new double[n];
			cPrime[0] = C[0] / B[0];
			for (int i = 1; i < n; i++)
				cPrime[i] = C[i] / (B[i] - cPrime[i - 1] * A[i]);

			double[] dPrime = new double[n];
			dPrime[0] = r[0] / B[0];
			for (int i = 1; i < n; i++)
				dPrime[i] = (r[i] - dPrime[i - 1] * A[i]) / (B[i] - cPrime[i - 1] * A[i]);

			double[] k = new double[n];
			k[n - 1] = dPrime[n - 1];
			for (int i = n - 2; i >= 0; i--)
				k[i] = dPrime[i] - cPrime[i] * k[i + 1];

			for (int i = 1; i < n; i++)
			{
				dx1 = x[i] - x[i - 1];
				dy1 = y[i] - y[i - 1];
				a[i - 1] = k[i - 1] * dx1 - dy1;
				b[i - 1] = -k[i] * dx1 + dy1;
			}

			return (a, b);
		}

		#endregion

		#region Catmull Rom

		private struct CatMullLast
		{
			public float c;
			public Matrix4x4 lastMatrix;
		}

		private static CatMullLast catMullLast;

		/// <summary>
		/// compute the Catmull Rom matrix
		/// </summary>
		/// <param name="c">correspond to the desired sharpness of the spline</param>
		private static Matrix4x4 GetCatmullMatrix(float c = 0.5f)
		{
			//matrix m0 in the form of an array of array
			var m0 = new float[4, 4]
			{
				{0, 1, 0, 0},
				{0, 0, 0, 0},
				{0, -3, 3, 0},
				{0, 2, -2, 0}
			};

			//matrix m1 in the form of an array of array
			var m1 = new float[4, 4]
			{
				{0, 1, 0, 0},
				{-1, 0, 1, 0},
				{2, -2, 1, -1},
				{-1, 1, -1, 1}
			};

			Matrix4x4 result = new Matrix4x4();
			for (int i = 0; i < 4; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					result[i, j] = (1 - c) * m0[j, i] + c * m1[j, i];
				}
			}

			return result;
		}

		/// <summary>
		/// return the results of the catmull rom polynomials in a Vector4
		/// </summary>
		/// <param name="u">correspond to the position between the points in [0,1]</param>
		/// <param name="c">correspond to the sharpness of the spline</param>
		/// <returns></returns>
		private static Vector4 GetCatmullPoly(float u, float c = 0.5f)
		{
			if (c != catMullLast.c || catMullLast.lastMatrix == Matrix4x4.zero)
			{
				catMullLast.c = c;
				catMullLast.lastMatrix = GetCatmullMatrix(c);
			}

			return (u < 0 || u > 1)
				? throw new ArgumentException("u < 0 or > 1")
				: catMullLast.lastMatrix * new Vector4(1, u, u * u, u * u * u);
		}

		/// <summary>
		/// Get the subpoint of a Catmull Rom spline going through the 4 points given in <paramref name="points"/> at the <paramref name="ratio"/>
		/// </summary>
		/// <param name="points">the 4 points needed to compute the spline</param>
		/// <param name="ratio">the position between [0,1]</param>
		/// <param name="sharpness">best in [0,1]</param>
		/// <returns>the point corresponding to the given spline at the given position</returns>
		private static Vector3 GetCatmullPoint(Vector3[] points, float ratio, float sharpness = 0.5f)
		{
			if (points.Length != 4) throw new ArgumentException($"{points.Length} must be == 4");
			Vector3 result = Vector3.zero;

			var poly = GetCatmullPoly(ratio, sharpness);

			for (int i = 0; i < 4; i++)
			{
				result += poly[i] * points[i];
			}

			return result;
		}

		/// <summary>
		/// compute all subpoints needed to make a Catmull Rom spline for a given array of points with the given precision and spline sharpness
		/// </summary>
		/// <param name="controlPoints">the points from which to compute the sline</param>
		/// <param name="precision">the desired precision</param>
		/// <param name="sharpness">the desired spline sharpness</param>
		/// <returns>the subpoints corresponding to the desired spline</returns>
		public static Vector3[] SplineCatmullRom(Vector3[] controlPoints, int precision = 10, float sharpness = 0.5f)
		{
			if (controlPoints.Length < 4)
				throw new ArgumentException($"{nameof(controlPoints)} must have more than 3 points");
			if (precision < 5) throw new ArgumentException($"{nameof(precision)} should be > 5");

			List<Vector3> result = new List<Vector3>();

			for (int i = 0; i < controlPoints.Length - 3; i++)
			{
				for (int j = 0; j < precision; j++)
				{
					result.Add(GetCatmullPoint(
							new[]
							{
								controlPoints[i],
								controlPoints[i + 1],
								controlPoints[i + 2],
								controlPoints[i + 3]
							},
							(float) j / precision,
							sharpness
						)
					);
				}
			}

			return result.ToArray();
		}

		public static void SetPointsForCatmullRomLoop<T>(ref T[] array)
		{
			Array.Resize(ref array, array.Length + 3);

			for (int i = 0; i < 3; ++i)
			{
				array[i + array.Length - 3] = array[i];
			}
		}

		#endregion
	}
}