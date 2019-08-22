using System;
using Newtonsoft.Json;
namespace DFMatrix {

    public class Matrix {
        public double[,] Data { get; protected set; }
        public int Rows { get; private set; }
        public int Cols { get; private set; }

        public int Length { get { return Cols * Rows; } }

        public Matrix(int Rows, int Cols) {
            this.Rows = Rows;
            this.Cols = Cols;
            this.Data = new double[Rows, Cols];
            this.Data.Initialize();
        }

        public virtual double this[int i, int j] {
            get { return Data[i, j]; }
            set { Data[i, j] = value; }
        }
        public override string ToString() {
            string result = "\n---------------\n";
            for (int i = 0; i < Rows; i++) {
                for (int j = 0; j < Cols; j++) {
                    result += string.Format(" {0} ", Data[i, j]);
                }
                result += "\n";
            }
            return result + "---------------\n";
        }

        public static bool operator ==(Matrix l, Matrix r) {
            return l.Equals(r);

        }

        public static bool operator !=(Matrix l, Matrix r) {
            return !(l == r);
        }
        public static Matrix operator -(Matrix l, Matrix r) {
            return Matrix.Subtract(l, r);
        }
        public static Matrix operator +(Matrix l, Matrix r) {
            return Matrix.Add(l, r);
        }
        public static Matrix operator *(Matrix l, Matrix r) {
            return Matrix.Multiply(l, r);
        }
        public static Matrix operator -(Matrix l, double r) {
            return Matrix.Subtract(l, r);
        }
        public static Matrix operator +(Matrix l, double r) {
            return Matrix.Add(l, r);
        }
        public static Matrix operator *(Matrix l, double r) {
            return Matrix.Scale(l, r);
        }
        public static implicit operator Matrix(double[,] d) {
            return new Matrix(d.GetLength(0), d.GetLength(1)) {
                Data = d
            };
        }

        public static implicit operator double[,] (Matrix d) {
            //double[,] data = d.Data;
            return d.Data;
        }

        public override bool Equals(object obj) {

            if (object.ReferenceEquals(obj, null)) {
                return object.ReferenceEquals(this, null);
            }

            Matrix r = (Matrix)obj;

            if (this.Rows != r.Rows || this.Cols != r.Cols) {
                return false;
            }
            for (int i = 0; i < this.Rows; i++) {
                for (int j = 0; j < this.Cols; j++) {
                    if (Math.Abs(this[i, j] - r[i, j]) > 0.0000001) {
                        return false;
                    }
                }
            }
            return true;
        }

        public override int GetHashCode() {
            int hash = 0;

            for (int i = 0; i < this.Rows; i++) {
                for (int j = 0; j < this.Cols; j++) {
                    hash ^= Data[i, j].GetHashCode();
                }
            }

            return hash;
        }
        public Matrix Copy() {
            return Matrix.Scale(this, 1);
        }
        public void Clear() {
            for (int i = 0; i < Rows; i++) {
                for (int j = 0; j < Cols; j++) {
                    this[i, j] = 0;
                }
            }
        }



        public string ToJson() {
            return JsonConvert.SerializeObject(this);
        }

        public static Matrix FromJson(string a) {
            return (Matrix)JsonConvert.DeserializeObject(a);
        }

        public void UpdateData(double[,] Data, int Rows, int Cols) {
            this.Data = Data;
            this.Rows = Rows;
            this.Cols = Cols;
        }

        public static Matrix FromArray(double[] a) {
            Matrix result = new Matrix(a.Length, 1);
            for (int i = 0; i < a.Length; i++) {
                result[i, 0] = a[i];
            }
            return result;
        }

        public static double[] ToArray(Matrix a) {
            double[] result = new double[a.Length];
            for (int i = 0; i < a.Rows; i++) {
                for (int j = 0; j < a.Cols; j++) {
                    double number = a[i, j];
                    result[i * (a.Rows - 1) + j] = number;
                }
            }
            return result;
        }

        public static Matrix Randomize(Matrix a, double Low, double High) {
            Random rand = new Random();
            Matrix result = new Matrix(a.Rows, a.Cols);
            for (int i = 0; i < result.Rows; i++) {
                for (int j = 0; j < result.Cols; j++) {
                    result[i, j] = (rand.NextDouble() * Math.Abs(High - Low) + Low);
                }
            }
            return result;
        }

        public static Matrix Randomize(Matrix a, double High) {
            Random rand = new Random();
            Matrix result = new Matrix(a.Rows, a.Cols);
            for (int i = 0; i < result.Rows; i++) {
                for (int j = 0; j < result.Cols; j++) {
                    result[i, j] = rand.NextDouble() * High;
                }
            }
            return result;
        }

        public static Matrix HadamardMultiply(Matrix a, Matrix b) {
            if (a.Rows != b.Rows || a.Cols != b.Cols) {
                throw new ArgumentException("Rows and Cols of A and B do not match.");
            }
            Matrix result = new Matrix(a.Rows, a.Cols);
            for (int i = 0; i < result.Rows; i++) {
                for (int j = 0; j < result.Cols; j++) {
                    result[i, j] = a[i, j] * b[i, j];
                }
            }
            return result;
        }

        public static Matrix Map(Matrix a, Func<double, double> func) {
            Matrix result = new Matrix(a.Rows, a.Cols);
            for (int i = 0; i < result.Rows; i++) {
                for (int j = 0; j < result.Cols; j++) {
                    result[i, j] = func(a[i, j]);
                }
            }
            return result;
        }

        public static Matrix SetAll(Matrix a, double num) {
            Matrix result = new Matrix(a.Rows, a.Cols);
            for (int i = 0; i < result.Rows; i++) {
                for (int j = 0; j < result.Cols; j++) {
                    result[i, j] = num;
                }
            }
            return result;
        }

        public static Matrix SetAll(Matrix a, int num) {
            return Matrix.SetAll(a, (double)num);
        }

        public static Matrix SetAll(Matrix a, float num) {
            return Matrix.SetAll(a, (double)num);
        }

        public static Matrix Add(Matrix a, Matrix b) {
            if (a.Rows != b.Rows || a.Cols != b.Cols) {
                throw new ArgumentException("Rows and Cols of A and B do not match.");
            }
            Matrix result = new Matrix(a.Rows, a.Cols);
            for (int i = 0; i < result.Rows; i++) {
                for (int j = 0; j < result.Cols; j++) {
                    result[i, j] = a[i, j] + b[i, j];
                }
            }
            return result;
        }

        public static Matrix Subtract(Matrix a, Matrix b) {
            if (a.Rows != b.Rows || a.Cols != b.Cols) {
                throw new ArgumentException("Rows and Cols of A and B do not match.");
            }
            Matrix result = new Matrix(a.Rows, a.Cols);
            for (int i = 0; i < result.Rows; i++) {
                for (int j = 0; j < result.Cols; j++) {
                    result[i, j] = a[i, j] - b[i, j];
                }
            }
            return result;
        }

        public static Matrix Add(Matrix a, double b) {

            Matrix result = new Matrix(a.Rows, a.Cols);
            for (int i = 0; i < result.Rows; i++) {
                for (int j = 0; j < result.Cols; j++) {
                    result[i, j] = a[i, j] + b;
                }
            }
            return result;
        }

        public static Matrix Subtract(Matrix a, double b) {

            Matrix result = new Matrix(a.Rows, a.Cols);
            for (int i = 0; i < result.Rows; i++) {
                for (int j = 0; j < result.Cols; j++) {
                    result[i, j] = a[i, j] - b;
                }
            }
            return result;
        }


        public static Matrix Multiply(Matrix a, Matrix b) {
            if (a.Cols != b.Rows) {
                throw new ArgumentException("Cols and Rows of A and B do not match. A Cols: " + a.Cols + " B Rows: " + b.Rows);
            }
            Matrix result = new Matrix(a.Rows, b.Cols);
            for (int i = 0; i < result.Rows; i++) {
                for (int j = 0; j < result.Cols; j++) {
                    double sum = 0;
                    for (int k = 0; k < a.Cols; k++) {
                        sum += a[i, k] * b[k, j];
                    }
                    result[i, j] = sum;
                }
            }
            return result;
        }

        public static Matrix Scale(Matrix a, double Scale) {
            Matrix result = new Matrix(a.Rows, a.Cols);
            for (int i = 0; i < result.Rows; i++) {
                for (int j = 0; j < result.Cols; j++) {
                    result[i, j] = a[i, j] * Scale;
                }
            }
            return result;
        }
        public static Matrix Transpose(Matrix a) {
            Matrix result = new Matrix(a.Cols, a.Rows);
            for (int i = 0; i < result.Rows; i++) {
                for (int j = 0; j < result.Cols; j++) {
                    result[i, j] = a[j, i];
                }
            }
            return result;
        }

        public static Vector MatToVec(Matrix a) {
            if (a.Rows > 1 && a.Cols > 1) {
                throw new ArgumentException("Cannot create vector from matrix with " + a.Rows + " rows and " + a.Cols + " cols.");
            }
            Vector vec = new Vector(a.Length);
            int count = 0;

            for (int i = 0; i < a.Rows; i++) {
                for (int j = 0; j < a.Cols; j++) {
                    vec[count] = a[i, j];
                    count++;
                }
            }

            return vec;
        }

        public static Matrix VecToMat(Vector a) {
            return (Matrix)a;
        }
        public static Matrix Clip(Matrix mat, double Min, double Max) {
            Matrix result = new Matrix(mat.Rows, mat.Cols);
            for (int i = 0; i < mat.Rows; i++) {
                for (int j = 0; j < mat.Cols; j++) {
                    if (mat[i, j] < Min) {
                        result[i, j] = Min;
                    }
                    else if (mat[i, j] > Max) {
                        result[i, j] = Max;
                    }
                }
            }
            return result;
        }
    }
}