import { Matrix2x2, Matrix3x3, Matrix4x4 } from '../src/Matrices';
import { Point, Vector } from '../src/PointVector';

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Matrix2x2

describe('Matrix2x2', () => {
  describe('ctor()', () => {
    it('should store the rows and columns', () => {
      const matrix = new Matrix2x2(1, 2, 3.5, 4.5);

      expect(matrix.m00).toBe(1);
      expect(matrix.m01).toBe(2);

      expect(matrix.m10).toBe(3.5);
      expect(matrix.m11).toBe(4.5);
    });
  });

  describe('isEqualTo()', () => {
    it('should be equal for identical matrices', () => {
      const matrix = new Matrix2x2(1, 2, 3, 4);
      expect(matrix.isEqualTo(matrix)).toBeTrue();
    });

    it('should be equal for roughly equivalient matrices', () => {
      const matrix1 = new Matrix2x2(1, 2, 3, 4);
      const matrix2 = new Matrix2x2(0.99999, 2.000001, 2.999999, 4.00001);
      expect(matrix1.isEqualTo(matrix2)).toBeTrue();
    });

    it('should not equal for different matrices', () => {
      const matrix1 = new Matrix2x2(1, 2, 3, 4);
      const matrix2 = new Matrix2x2(5, 6, 7, 8);
      expect(matrix1.isEqualTo(matrix2)).toBeFalse();
    });
  });

  describe('determinant()', () => {
    it('should return the determinant', () => {
      const matrix = new Matrix2x2(1, 5, -3, 2);
      expect(matrix.determinant()).toBe(17);
    });
  });
});

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Matrix3x3

describe('Matrix3x3', () => {
  describe('ctor()', () => {
    it('should store the rows and columns', () => {
      // prettier-ignore
      const matrix = new Matrix3x3(
        1,   2,   3,
        4.5, 5.5, 6.5,
        7,   8,   9);
      expect(matrix.m00).toBe(1);
      expect(matrix.m01).toBe(2);
      expect(matrix.m02).toBe(3);

      expect(matrix.m10).toBe(4.5);
      expect(matrix.m11).toBe(5.5);
      expect(matrix.m12).toBe(6.5);

      expect(matrix.m20).toBe(7);
      expect(matrix.m21).toBe(8);
      expect(matrix.m22).toBe(9);
    });
  });

  describe('isEqualTo()', () => {
    it('should be equal for identical matrices', () => {
      const matrix = new Matrix3x3(1, 2, 3, 4, 5, 6, 7, 8, 9);
      expect(matrix.isEqualTo(matrix)).toBeTrue();
    });

    it('should be equal for roughly equivalient matrices', () => {
      // prettier-ignore
      const matrix1 = new Matrix3x3(
        1, 2, 3,
        4, 5, 6,
        7, 8, 9);

      // prettier-ignore
      const matrix2 = new Matrix3x3(
        0.99999, 2.000001, 2.999999,
        4.00001, 4.99999,  6.00001,
        6.99999, 8.00001,  8.99999);
      expect(matrix1.isEqualTo(matrix2)).toBeTrue();
    });

    it('should not equal for different matrices', () => {
      const matrix1 = new Matrix3x3(1, 2, 3, 4, 5, 6, 7, 8, 9);
      const matrix2 = new Matrix3x3(10, 20, 30, 40, 50, 60, 70, 80, 90);
      expect(matrix1.isEqualTo(matrix2)).toBeFalse();
    });
  });

  describe('submatrix()', () => {
    it('should remove the specified row and column and return a 2x2 matrix', () => {
      // prettier-ignore
      const matrix = new Matrix3x3(
        1, 5, 0,
        -3, 2, 7,
        0, 6, -3);
      expect(matrix.submatrix(0, 0)).toEqual(new Matrix2x2(2, 7, 6, -3));
      expect(matrix.submatrix(0, 1)).toEqual(new Matrix2x2(-3, 7, 0, -3));
      expect(matrix.submatrix(0, 2)).toEqual(new Matrix2x2(-3, 2, 0, 6));

      expect(matrix.submatrix(1, 0)).toEqual(new Matrix2x2(5, 0, 6, -3));
      expect(matrix.submatrix(1, 1)).toEqual(new Matrix2x2(1, 0, 0, -3));
      expect(matrix.submatrix(1, 2)).toEqual(new Matrix2x2(1, 5, 0, 6));

      expect(matrix.submatrix(2, 0)).toEqual(new Matrix2x2(5, 0, 2, 7));
      expect(matrix.submatrix(2, 1)).toEqual(new Matrix2x2(1, 0, -3, 7));
      expect(matrix.submatrix(2, 2)).toEqual(new Matrix2x2(1, 5, -3, 2));
    });
  });

  describe('minor()', () => {
    it('should return the minor of the matrix, which is the determinant of the submatrix', () => {
      // prettier-ignore
      const matrix = new Matrix3x3(
        3, 5, 0,
        2, -1, -7,
        6, -1, 5);

      expect(matrix.minor(0, 0)).toBe(-12);
      expect(matrix.minor(0, 1)).toBe(52);
      expect(matrix.minor(0, 2)).toBe(4);

      expect(matrix.minor(1, 0)).toBe(25);
      expect(matrix.minor(1, 1)).toBe(15);
      expect(matrix.minor(1, 2)).toBe(-33);

      expect(matrix.minor(2, 0)).toBe(-35);
      expect(matrix.minor(2, 1)).toBe(-21);
      expect(matrix.minor(2, 2)).toBe(-13);
    });
  });

  describe('cofactor()', () => {
    it('should return the cofactor of the matrix', () => {
      // prettier-ignore
      const matrix = new Matrix3x3(
        3, 5, 0,
        2, -1, -7,
        6, -1, 5);

      expect(matrix.cofactor(0, 0)).toBe(-12);
      expect(matrix.cofactor(0, 1)).toBe(-52);
      expect(matrix.cofactor(0, 2)).toBe(4);

      expect(matrix.cofactor(1, 0)).toBe(-25);
      expect(matrix.cofactor(1, 1)).toBe(15);
      expect(matrix.cofactor(1, 2)).toBe(33);

      expect(matrix.cofactor(2, 0)).toBe(-35);
      expect(matrix.cofactor(2, 1)).toBe(21);
      expect(matrix.cofactor(2, 2)).toBe(-13);
    });

    describe('determinant()', () => {
      it('should return the determinant of the matrix', () => {
        // prettier-ignore
        const matrix = new Matrix3x3(
          1, 2, 6,
          -5, 8, -4,
          2, 6, 4);

        expect(matrix.determinant()).toBe(-196);
      });
    });
  });
});

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Matrix4x4

describe('Matrix4x4', () => {
  describe('ctor()', () => {
    it('should store the rows and columns', () => {
      // prettier-ignore
      const matrix = new Matrix4x4(
        1,    2,    3,    4,
        5.5,  6.5,  7.5,  8.5,
        9,    10,   11,   12,
        13.5, 14.5, 15.5, 16.5);

      expect(matrix.m00).toBe(1);
      expect(matrix.m01).toBe(2);
      expect(matrix.m02).toBe(3);
      expect(matrix.m03).toBe(4);

      expect(matrix.m10).toBe(5.5);
      expect(matrix.m11).toBe(6.5);
      expect(matrix.m12).toBe(7.5);
      expect(matrix.m13).toBe(8.5);

      expect(matrix.m20).toBe(9);
      expect(matrix.m21).toBe(10);
      expect(matrix.m22).toBe(11);
      expect(matrix.m23).toBe(12);

      expect(matrix.m30).toBe(13.5);
      expect(matrix.m31).toBe(14.5);
      expect(matrix.m32).toBe(15.5);
      expect(matrix.m33).toBe(16.5);
    });
  });

  describe('isEqualTo()', () => {
    it('should be equal for identical matrices', () => {
      const matrix = new Matrix4x4(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
      expect(matrix.isEqualTo(matrix)).toBeTrue();
    });

    it('should be equal for roughly equivalient matrices', () => {
      // prettier-ignore
      const matrix1 = new Matrix4x4(
        1,  2,  3,  4,
        5,  6,  7,  8,
        9,  10, 11, 12,
        13, 14, 15, 16);

      // prettier-ignore
      const matrix2 = new Matrix4x4(
        0.99999,  2.000001, 2.999999, 4.00001,
        4.99999,  6.00001,  6.99999,  8.00001,
        8.99999,  10.00001, 10.99999, 12.00001,
        12.99999, 14.00001, 14.99999, 16.00001,
      );
      expect(matrix1.isEqualTo(matrix2)).toBeTrue();
    });

    it('should not equal for different matrices', () => {
      const matrix1 = new Matrix4x4(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
      const matrix2 = new Matrix4x4(10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150, 160);
      expect(matrix1.isEqualTo(matrix2)).toBeFalse();
    });
  });

  describe('multiply()', () => {
    it('should multiple two matrices', () => {
      // prettier-ignore
      const matrix1 = new Matrix4x4(
        1, 2, 3, 4,
        5, 6, 7, 8,
        9, 8, 7, 6,
        5, 4, 3, 2);

      // prettier-ignore
      const matrix2 = new Matrix4x4(
        -2, 1, 2,  3,
         3, 2, 1, -1,
         4, 3, 6,  5,
         1, 2, 7,  8);

      expect(matrix1.multiply(matrix2)).toEqual(
        // prettier-ignore
        new Matrix4x4(
          20, 22, 50,  48,
          44, 54, 114, 108,
          40, 58, 110, 102,
          16, 26, 46,  42),
      );
    });

    it('should multiply a matrix and a point', () => {
      // prettier-ignore
      const matrix = new Matrix4x4(
        1, 2, 3, 4,
        2, 4, 4, 2,
        8, 6, 4, 1,
        0, 0, 0, 1);

      const point = new Point(1, 2, 3);
      expect(matrix.multiplyByPoint(point)).toEqual(new Point(18, 24, 33));
    });

    it('should multiply a matrix and a vector', () => {
      // prettier-ignore
      const matrix = new Matrix4x4(
        1, 2, 3, 4,
        2, 4, 4, 2,
        8, 6, 4, 1,
        0, 0, 0, 1);

      const vector = new Vector(1, 2, 3);
      expect(matrix.multiplyByVector(vector)).toEqual(new Vector(14, 22, 32));
    });

    it('should multiply a matrix by the identity matrix and return the same matrix', () => {
      // prettier-ignore
      const matrix = new Matrix4x4(
        0, 1, 2,  4,
        1, 2, 4,  8,
        2, 4, 8,  16,
        4, 8, 16, 32);

      expect(matrix.multiply(Matrix4x4.identity)).toEqual(matrix);
    });

    it('should multiply the identity matrix by a point and return the same point', () => {
      const p = new Point(1, 2, 3);
      expect(Matrix4x4.identity.multiplyByPoint(p)).toEqual(p);
    });

    it('should multiply the identity matrix by a vector and return the same vector', () => {
      const v = new Vector(1, 2, 3);
      expect(Matrix4x4.identity.multiplyByVector(v)).toEqual(v);
    });
  });

  describe('transpose()', () => {
    it('should swap the rows and columns', () => {
      // prettier-ignore
      const matrix = new Matrix4x4(
        0, 9, 3, 0,
        9, 8, 0, 8,
        1, 8, 5, 3,
        0, 0, 5, 8);

      // prettier-ignore
      const expectedTransposed = new Matrix4x4(
        0, 9, 1, 0,
        9, 8, 8, 0,
        3, 0, 5, 5,
        0, 8, 3, 8);

      expect(matrix.transpose()).toEqual(expectedTransposed);
    });

    it('should return the identity matrix when the identity is transposed', () => {
      expect(Matrix4x4.identity.transpose()).toEqual(Matrix4x4.identity);
    });
  });

  describe('submatrix()', () => {
    it('should remove the specified row and column and return a 3x3 matrix ', () => {
      // prettier-ignore
      const matrix = new Matrix4x4(
        -6, 1, 1, 6,
        -8, 5, 8, 6,
        -1, 0, 8, 2,
        -7, 1, -1, 1);

      expect(matrix.submatrix(0, 0)).toEqual(new Matrix3x3(5, 8, 6, 0, 8, 2, 1, -1, 1));
      expect(matrix.submatrix(0, 1)).toEqual(new Matrix3x3(-8, 8, 6, -1, 8, 2, -7, -1, 1));
      expect(matrix.submatrix(0, 2)).toEqual(new Matrix3x3(-8, 5, 6, -1, 0, 2, -7, 1, 1));
      expect(matrix.submatrix(0, 3)).toEqual(new Matrix3x3(-8, 5, 8, -1, 0, 8, -7, 1, -1));

      expect(matrix.submatrix(1, 0)).toEqual(new Matrix3x3(1, 1, 6, 0, 8, 2, 1, -1, 1));
      expect(matrix.submatrix(1, 1)).toEqual(new Matrix3x3(-6, 1, 6, -1, 8, 2, -7, -1, 1));
      expect(matrix.submatrix(1, 2)).toEqual(new Matrix3x3(-6, 1, 6, -1, 0, 2, -7, 1, 1));
      expect(matrix.submatrix(1, 3)).toEqual(new Matrix3x3(-6, 1, 1, -1, 0, 8, -7, 1, -1));

      expect(matrix.submatrix(2, 0)).toEqual(new Matrix3x3(1, 1, 6, 5, 8, 6, 1, -1, 1));
      expect(matrix.submatrix(2, 1)).toEqual(new Matrix3x3(-6, 1, 6, -8, 8, 6, -7, -1, 1));
      expect(matrix.submatrix(2, 2)).toEqual(new Matrix3x3(-6, 1, 6, -8, 5, 6, -7, 1, 1));
      expect(matrix.submatrix(2, 3)).toEqual(new Matrix3x3(-6, 1, 1, -8, 5, 8, -7, 1, -1));

      expect(matrix.submatrix(3, 0)).toEqual(new Matrix3x3(1, 1, 6, 5, 8, 6, 0, 8, 2));
      expect(matrix.submatrix(3, 1)).toEqual(new Matrix3x3(-6, 1, 6, -8, 8, 6, -1, 8, 2));
      expect(matrix.submatrix(3, 2)).toEqual(new Matrix3x3(-6, 1, 6, -8, 5, 6, -1, 0, 2));
      expect(matrix.submatrix(3, 3)).toEqual(new Matrix3x3(-6, 1, 1, -8, 5, 8, -1, 0, 8));
    });
  });

  describe('minor()', () => {
    it('should return the minor of the matrix, which is the determinant of the submatrix', () => {
      // prettier-ignore
      const matrix = new Matrix4x4(
        3, 5, 0, 4,
        2, -1, -7, 9,
        6, -1, 5, 2,
        3, 7, -4, 6);

      expect(matrix.minor(0, 0)).toBe(-457);
      expect(matrix.minor(0, 1)).toBe(-65);
      expect(matrix.minor(0, 2)).toBe(395);
      expect(matrix.minor(0, 3)).toBe(-416);

      expect(matrix.minor(1, 0)).toBe(66);
      expect(matrix.minor(1, 1)).toBe(-42);
      expect(matrix.minor(1, 2)).toBe(-30);
      expect(matrix.minor(1, 3)).toBe(102);

      expect(matrix.minor(2, 0)).toBe(182);
      expect(matrix.minor(2, 1)).toBe(34);
      expect(matrix.minor(2, 2)).toBe(-64);
      expect(matrix.minor(2, 3)).toBe(94);
    });
  });

  describe('cofactor()', () => {
    it('should return the cofactor of the matrix', () => {
      // prettier-ignore
      const matrix = new Matrix4x4(
        3, 5, 0, 4,
        2, -1, -7, 9,
        6, -1, 5, 2,
        3, 7, -4, 6);

      expect(matrix.cofactor(0, 0)).toBe(-457);
      expect(matrix.cofactor(0, 1)).toBe(65);
      expect(matrix.cofactor(0, 2)).toBe(395);
      expect(matrix.cofactor(0, 3)).toBe(416);

      expect(matrix.cofactor(1, 0)).toBe(-66);
      expect(matrix.cofactor(1, 1)).toBe(-42);
      expect(matrix.cofactor(1, 2)).toBe(30);
      expect(matrix.cofactor(1, 3)).toBe(102);

      expect(matrix.cofactor(2, 0)).toBe(182);
      expect(matrix.cofactor(2, 1)).toBe(-34);
      expect(matrix.cofactor(2, 2)).toBe(-64);
      expect(matrix.cofactor(2, 3)).toBe(-94);
    });
  });

  describe('determinant()', () => {
    it('should return the determinant of the matrix', () => {
      // prettier-ignore
      const matrix = new Matrix4x4(
        -2, -8, 3, 5,
        -3, 1, 7, 3,
        1, 2, -9, 6,
        -6, 7, 7, -9);

      expect(matrix.determinant()).toBe(-4071);
    });
  });

  describe('isInvertible()', () => {
    it('if the determinant is non-zero the matrix is invertible', () => {
      // prettier-ignore
      const matrix = new Matrix4x4(
        6, 4, 4, 4,
        5, 5, 7, 6,
        4, -9, 3, -7,
        9, 1, 7, -6);

      expect(matrix.determinant()).toBe(-2120);
      expect(matrix.isInvertible()).toBeTrue();
    });

    it('if the derminant is zero the matrix is non-invertible', () => {
      // prettier-ignore
      const matrix = new Matrix4x4(
        -4, 2, -2, -3,
        9, 6, 2, 6,
        0, -5, 1, -5,
        0, 0, 0, 0);

      expect(matrix.determinant()).toBe(0);
      expect(matrix.isInvertible()).toBeFalse();
    });
  });

  describe('inverse()', () => {
    it('should invert the matrix', () => {
      // prettier-ignore
      const matrix = new Matrix4x4(
        -5, 2, 6, -8,
        1, -5, 1, 8,
        7, 7, -6, -7,
        1, -3, 7, 4);

      // prettier-ignore
      const invertedMatrix = new Matrix4x4(
        0.21805, 0.45113, 0.24060, -0.04511,
        -0.80827, -1.45677, -0.44361,0.52068,
        -0.07895, -0.22368, -0.05263, 0.19737,
        -0.52256, -0.81391, -0.30075, 0.30639);

      expect(matrix.inverse().isEqualTo(invertedMatrix)).toBeTrue();
    });

    it('should invert another matrix', () => {
      // prettier-ignore
      const matrix = new Matrix4x4(
        8, -5, 9, 2,
        7, 5, 6, 1,
        -6, 0, 9, 6,
        -3, 0, -9, -4);

      // prettier-ignore
      const invertedMatrix = new Matrix4x4(
        -0.15385, -0.15385, -0.28205, -0.53846,
        -0.07692, 0.12308, 0.02564, 0.03077,
        0.35897, 0.35897, 0.43590, 0.92308,
        -0.69231, -0.69231, -0.76923, -1.92308);

      expect(matrix.inverse().isEqualTo(invertedMatrix)).toBeTrue();
    });

    it('should invert a third matrix', () => {
      // prettier-ignore
      const matrix = new Matrix4x4(
        9, 3, 0, 9,
        -5, -2, -6, -3,
        -4, 9, 6, 4,
        -7, 6, 6, 2);

      // prettier-ignore
      const invertedMatrix = new Matrix4x4(
        -0.04074, -0.07778, 0.14444, -0.22222,
        -0.07778, 0.03333, 0.36667, -0.33333,
        -0.02901, -0.14630, -0.10926, 0.12963,
        0.17778, 0.06667, -0.26667, 0.33333);

      expect(matrix.inverse().isEqualTo(invertedMatrix)).toBeTrue();
    });

    it('should throw when trying to invert the matrix if it is not invertible', () => {
      // prettier-ignore
      const matrix = new Matrix4x4(
        -4, 2, -2, -3,
        9, 6, 2, 6,
        0, -5, 1, -5,
        0, 0, 0, 0);

      expect(() => matrix.inverse()).toThrow();
    });

    it('should return the same matrix if multiplying a product by its inverse', () => {
      // prettier-ignore
      const a = new Matrix4x4(
        3, -9, 7, 3,
        3, -8, 2, -9,
        -4, 4, 4, 1,
        -6, 5, -1, 1);

      // prettier-ignore
      const b = new Matrix4x4(
        8, 2, 2, 2,
        3, -1, 7, 0,
        7, 0, 5, 4,
        6, -2, 0, 5);

      const c = a.multiply(b);
      expect(c.multiply(b.inverse()).isEqualTo(a)).toBeTrue();
    });
  });
});
