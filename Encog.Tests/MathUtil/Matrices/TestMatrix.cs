//
// Encog(tm) Core v3.3 - .Net Version (unit test)
// http://www.heatonresearch.com/encog/
//
// Copyright 2008-2014 Heaton Research, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//   
// For more information on Heaton Research copyrights, licenses 
// and trademarks visit:
// http://www.heatonresearch.com/copyright
//
using NUnit.Framework;

namespace Encog.MathUtil.Matrices
{
    [TestFixture]
    public class TestMatrix
    {
        [Test]
        public void RowsAndCols()
        {
            var matrix = new Matrix(6, 3);
            Assert.AreEqual(matrix.Rows, 6);
            Assert.AreEqual(matrix.Cols, 3);

            matrix[1, 2] = 1.5;
            Assert.AreEqual(matrix[1, 2], 1.5);
        }

        [Test]
        public void RowAndColRangeUnder()
        {
            var matrix = new Matrix(6, 3);

            // make sure set registers error on under-bound row
            try
            {
                matrix[-1, 0] = 1;
                Assert.IsTrue(false); // should have thrown an exception
            }
            catch (MatrixError)
            {
            }

            // make sure set registers error on under-bound col
            try
            {
                matrix[0, -1] = 1;
                Assert.IsTrue(false); // should have thrown an exception
            }
            catch (MatrixError)
            {
            }

            // make sure get registers error on under-bound row
            try
            {
                double d = matrix[-1, 0];
                matrix[0, 0] = d;
                Assert.IsTrue(false); // should have thrown an exception
            }
            catch (MatrixError)
            {
            }

            // make sure set registers error on under-bound col
            try
            {
                double d = matrix[0, -1];
                matrix[0, 0] = d;
                Assert.IsTrue(false); // should have thrown an exception
            }
            catch (MatrixError)
            {
            }
        }

        [Test]
        public void RowAndColRangeOver()
        {
            var matrix = new Matrix(6, 3);

            // make sure set registers error on under-bound row
            try
            {
                matrix[6, 0] = 1;
                Assert.IsTrue(false); // should have thrown an exception
            }
            catch (MatrixError)
            {
            }

            // make sure set registers error on under-bound col
            try
            {
                matrix[0, 3] = 1;
                Assert.IsTrue(false); // should have thrown an exception
            }
            catch (MatrixError)
            {
            }

            // make sure get registers error on under-bound row
            try
            {
                double d = matrix[6, 0];
                matrix[0, 0] = d;
                Assert.IsTrue(false); // should have thrown an exception
            }
            catch (MatrixError)
            {
            }

            // make sure set registers error on under-bound col
            try
            {
                double d = matrix[0, 3];
                matrix[0, 0] = d;
                Assert.IsTrue(false); // should have thrown an exception
            }
            catch (MatrixError)
            {
            }
        }

        [Test]
        public void MatrixConstruct()
        {
            double[][] m = {
                               new[] {1.0, 2.0, 3.0, 4.0},
                               new[] {5.0, 6.0, 7.0, 8.0},
                               new[] {9.0, 10.0, 11.0, 12.0},
                               new[] {13.0, 14.0, 15.0, 16.0}
                           };
            var matrix = new Matrix(m);
            Assert.AreEqual(matrix.Rows, 4);
            Assert.AreEqual(matrix.Cols, 4);
        }

        [Test]
        public void MatrixEquals()
        {
            double[][] m1 = {
                                new[] {1.0, 2.0},
                                new[] {3.0, 4.0}
                            };

            double[][] m2 = {
                                new[] {0.0, 2.0},
                                new[] {3.0, 4.0}
                            };

            var matrix1 = new Matrix(m1);
            var matrix2 = new Matrix(m1);

            Assert.IsTrue(matrix1.Equals(matrix2));

            matrix2 = new Matrix(m2);

            Assert.IsFalse(matrix1.Equals(matrix2));
        }

        [Test]
        public void MatrixEqualsPrecision()
        {
            double[][] m1 = {
                                new[] {1.1234, 2.123},
                                new[] {3.123, 4.123}
                            };

            double[][] m2 = {
                                new[] {1.123, 2.123},
                                new[] {3.123, 4.123}
                            };

            var matrix1 = new Matrix(m1);
            var matrix2 = new Matrix(m2);

            Assert.IsTrue(matrix1.equals(matrix2, 3));
            Assert.IsFalse(matrix1.equals(matrix2, 4));

            double[][] m3 = {
                                new[] {1.1, 2.1},
                                new[] {3.1, 4.1}
                            };

            double[][] m4 = {
                                new[] {1.2, 2.1},
                                new[] {3.1, 4.1}
                            };

            var matrix3 = new Matrix(m3);
            var matrix4 = new Matrix(m4);
            Assert.IsTrue(matrix3.equals(matrix4, 0));
            Assert.IsFalse(matrix3.equals(matrix4, 1));

            try
            {
                matrix3.equals(matrix4, -1);
                Assert.IsTrue(false);
            }
            catch (MatrixError)
            {
            }

            try
            {
                matrix3.equals(matrix4, 19);
                Assert.IsTrue(false);
            }
            catch (MatrixError)
            {
            }
        }

        [Test]
        public void MatrixMultiply()
        {
            double[][] a = {
                               new[] {1.0, 0.0, 2.0},
                               new[] {-1.0, 3.0, 1.0}
                           };

            double[][] b = {
                               new[] {3.0, 1.0},
                               new[] {2.0, 1.0},
                               new[] {1.0, 0.0}
                           };

            double[][] c = {
                               new[] {5.0, 1.0},
                               new[] {4.0, 2.0}
                           };

            var matrixA = new Matrix(a);
            var matrixB = new Matrix(b);
            var matrixC = new Matrix(c);

            var result = (Matrix) matrixA.Clone();
            result.ToString();
            result = MatrixMath.Multiply(matrixA, matrixB);

            Assert.IsTrue(result.Equals(matrixC));

            double[][] a2 = {
                                new[] {1.0, 2.0, 3.0, 4.0},
                                new[] {5.0, 6.0, 7.0, 8.0}
                            };

            double[][] b2 = {
                                new[] {1.0, 2.0, 3.0},
                                new[] {4.0, 5.0, 6.0},
                                new[] {7.0, 8.0, 9.0},
                                new[] {10.0, 11.0, 12.0}
                            };

            double[][] c2 = {
                                new[] {70.0, 80.0, 90.0},
                                new[] {158.0, 184.0, 210.0}
                            };

            matrixA = new Matrix(a2);
            matrixB = new Matrix(b2);
            matrixC = new Matrix(c2);

            result = MatrixMath.Multiply(matrixA, matrixB);
            Assert.IsTrue(result.Equals(matrixC));

            matrixB.Clone();
            
            try
            {
                MatrixMath.Multiply(matrixB, matrixA);
                Assert.IsTrue(false);
            }
            catch (MatrixError)
            {
            }
        }

        [Test]
        public void Boolean()
        {
            bool[][] matrixDataBoolean = {
                                             new[] {true, false},
                                             new[] {false, true}
                                         };

            double[][] matrixDataDouble = {
                                              new[] {1.0, -1.0},
                                              new[] {-1.0, 1.0},
                                          };

            var matrixBoolean = new Matrix(matrixDataBoolean);
            var matrixDouble = new Matrix(matrixDataDouble);

            Assert.IsTrue(matrixBoolean.Equals(matrixDouble));
        }

        [Test]
        public void GetRow()
        {
            double[][] matrixData1 = {
                                         new[] {1.0, 2.0},
                                         new[] {3.0, 4.0}
                                     };
            double[][] matrixData2 = {
                                         new[] {3.0, 4.0}
                                     };

            var matrix1 = new Matrix(matrixData1);
            var matrix2 = new Matrix(matrixData2);

            Matrix matrixRow = matrix1.GetRow(1);
            Assert.IsTrue(matrixRow.Equals(matrix2));

            try
            {
                matrix1.GetRow(3);
                Assert.IsTrue(false);
            }
            catch (MatrixError)
            {
                Assert.IsTrue(true);
            }
        }

        [Test]
        public void GetCol()
        {
            double[][] matrixData1 = {
                                         new[] {1.0, 2.0},
                                         new[] {3.0, 4.0}
                                     };
            double[][] matrixData2 = {
                                         new[] {2.0},
                                         new[] {4.0}
                                     };

            var matrix1 = new Matrix(matrixData1);
            var matrix2 = new Matrix(matrixData2);

            Matrix matrixCol = matrix1.GetCol(1);
            Assert.IsTrue(matrixCol.Equals(matrix2));

            try
            {
                matrix1.GetCol(3);
                Assert.IsTrue(false);
            }
            catch (MatrixError)
            {
                Assert.IsTrue(true);
            }
        }

        [Test]
        public void Zero()
        {
            double[][] doubleData = {
                                        new[] {0.0, 0.0},
                                        new[] {0.0, 0.0}
                                    };
            var matrix = new Matrix(doubleData);
            Assert.IsTrue(matrix.IsZero());
        }

        [Test]
        public void Sum()
        {
            double[][] doubleData = {
                                        new[] {1.0, 2.0},
                                        new[] {3.0, 4.0}
                                    };
            var matrix = new Matrix(doubleData);
            Assert.AreEqual((int) matrix.Sum(), 1 + 2 + 3 + 4);
        }

        [Test]
        public void RowMatrix()
        {
            double[] matrixData = {1.0, 2.0, 3.0, 4.0};
            Matrix matrix = Matrix.CreateRowMatrix(matrixData);
            Assert.AreEqual(matrix[0, 0], 1.0);
            Assert.AreEqual(matrix[0, 1], 2.0);
            Assert.AreEqual(matrix[0, 2], 3.0);
            Assert.AreEqual(matrix[0, 3], 4.0);
        }

        [Test]
        public void ColumnMatrix()
        {
            double[] matrixData = {1.0, 2.0, 3.0, 4.0};
            Matrix matrix = Matrix.CreateColumnMatrix(matrixData);
            Assert.AreEqual(matrix[0, 0], 1.0);
            Assert.AreEqual(matrix[1, 0], 2.0);
            Assert.AreEqual(matrix[2, 0], 3.0);
            Assert.AreEqual(matrix[3, 0], 4.0);
        }

        [Test]
        public void Add()
        {
            double[] matrixData = {1.0, 2.0, 3.0, 4.0};
            Matrix matrix = Matrix.CreateColumnMatrix(matrixData);
            matrix.Add(0, 0, 1);
            Assert.AreEqual(matrix[0, 0], 2.0);
        }

        [Test]
        public void Clear()
        {
            double[] matrixData = {1.0, 2.0, 3.0, 4.0};
            Matrix matrix = Matrix.CreateColumnMatrix(matrixData);
            matrix.Clear();
            Assert.AreEqual(matrix[0, 0], 0.0);
            Assert.AreEqual(matrix[1, 0], 0.0);
            Assert.AreEqual(matrix[2, 0], 0.0);
            Assert.AreEqual(matrix[3, 0], 0.0);
        }

        [Test]
        public void IsVector()
        {
            double[] matrixData = {1.0, 2.0, 3.0, 4.0};
            Matrix matrixCol = Matrix.CreateColumnMatrix(matrixData);
            Matrix matrixRow = Matrix.CreateRowMatrix(matrixData);
            Assert.IsTrue(matrixCol.IsVector());
            Assert.IsTrue(matrixRow.IsVector());
            double[][] matrixData2 = {
                                         new[] {1.0, 2.0},
                                         new[] {3.0, 4.0}
                                     };
            var matrix = new Matrix(matrixData2);
            Assert.IsFalse(matrix.IsVector());
        }

        [Test]
        public void IsZero()
        {
            double[] matrixData = {1.0, 2.0, 3.0, 4.0};
            Matrix matrix = Matrix.CreateColumnMatrix(matrixData);
            Assert.IsFalse(matrix.IsZero());
            double[] matrixData2 = {0.0, 0.0, 0.0, 0.0};
            Matrix matrix2 = Matrix.CreateColumnMatrix(matrixData2);
            Assert.IsTrue(matrix2.IsZero());
        }

        [Test]
        public void PackedArray()
        {
            double[][] matrixData = {
                                        new[] {1.0, 2.0},
                                        new[] {3.0, 4.0}
                                    };
            var matrix = new Matrix(matrixData);
            double[] matrixData2 = matrix.ToPackedArray();
            Assert.AreEqual(4, matrixData2.Length);
            Assert.AreEqual(1.0, matrix[0, 0]);
            Assert.AreEqual(2.0, matrix[0, 1]);
            Assert.AreEqual(3.0, matrix[1, 0]);
            Assert.AreEqual(4.0, matrix[1, 1]);

            var matrix2 = new Matrix(2, 2);
            matrix2.FromPackedArray(matrixData2, 0);
            Assert.IsTrue(matrix.Equals(matrix2));
        }

        [Test]
        public void PackedArray2()
        {
            double[] data = {1.0, 2.0, 3.0, 4.0};
            var matrix = new Matrix(1, 4);
            matrix.FromPackedArray(data, 0);
            Assert.AreEqual(1.0, matrix[0, 0]);
            Assert.AreEqual(2.0, matrix[0, 1]);
            Assert.AreEqual(3.0, matrix[0, 2]);
        }

        [Test]
        public void Size()
        {
            double[][] data = {
                                  new[] {1.0, 2.0},
                                  new[] {3.0, 4.0}
                              };
            var matrix = new Matrix(data);
            Assert.AreEqual(4, matrix.Size);
        }

        [Test]
        public void Randomize()
        {
            const double min = 1.0;
            const double max = 10.0;
            var matrix = new Matrix(10, 10);
            matrix.Ramdomize(min, max);
            var array = matrix.ToPackedArray();
            foreach (double t in array)
            {
                if (t < min || t > max)
                    Assert.IsFalse(true);
            }
        }

        [Test]
        public void VectorLength()
        {
            double[] vectorData = {1.0, 2.0, 3.0, 4.0};
            Matrix vector = Matrix.CreateRowMatrix(vectorData);
            Assert.AreEqual(5, (int) MatrixMath.VectorLength(vector));

            var nonVector = new Matrix(2, 2);
            try
            {
                MatrixMath.VectorLength(nonVector);
                Assert.IsTrue(false);
            }
            catch (MatrixError)
            {
            }
        }
    }
}
