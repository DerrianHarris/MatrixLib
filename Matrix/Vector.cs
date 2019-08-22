using System;
namespace DFMatrix {
    public class Vector : Matrix {
        public Vector(int i) : base(i, 1) {
        }

        public double this[int i] {
            get { return this[i, 0]; }
            set { this[i, 0] = value; }
        }
        private new double this[int i, int j] {
            get { return Data[i, j]; }
            set { Data[i, j] = value; }
        }

        public double Sum() {
            double sum = 0;
            for (int i = 0; i < Length; i++) {
                sum += this[i];
            }
            return sum;
        }

        public double Mean() {
            return Sum() / (double)Length;
        }

        public static Vector operator -(Vector l, Matrix r) {
            return Matrix.MatToVec(Matrix.Subtract(l, r));
        }
        public static Vector operator +(Vector l, Matrix r) {
            return Matrix.MatToVec(Matrix.Add(l, r));
        }
        public static Vector operator *(Vector l, Matrix r) {
            return Matrix.MatToVec(Matrix.Multiply(l, r));
        }
        public static Vector operator -(Vector l, double r) {
            return Matrix.MatToVec(Matrix.Subtract(l, r));
        }
        public static Vector operator +(Vector l, double r) {
            return Matrix.MatToVec(Matrix.Add(l, r));
        }
        public static Vector operator *(Vector l, double r) {
            return Matrix.MatToVec(Matrix.Scale(l, r));
        }
        public static implicit operator Vector(double[,] d) {
            return new Vector(d.GetLength(0)) {
                Data = d
            };
        }

        public static implicit operator double[,] (Vector d) {
            //double[,] data = d.Data;
            return d.Data;
        }

    }
}
