import { Matrix2x2, Matrix3x3, Matrix4x4 } from '../src/Matrices';

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
      expect(matrix.isEqualTo(matrix)).toBe(true);
    });

    it('should be equal for roughly equivalient matrices', () => {
      const matrix1 = new Matrix2x2(1, 2, 3, 4);
      const matrix2 = new Matrix2x2(0.99999, 2.000001, 2.999999, 4.00001);
      expect(matrix1.isEqualTo(matrix2)).toBe(true);
    });

    it('should not equal for different matrices', () => {
      const matrix1 = new Matrix2x2(1, 2, 3, 4);
      const matrix2 = new Matrix2x2(5, 6, 7, 8);
      expect(matrix1.isEqualTo(matrix2)).toBe(false);
    });
  });
});

describe('Matrix3x3', () => {
  describe('ctor()', () => {
    it('should store the rows and columns', () => {
      const matrix = new Matrix3x3(1, 2, 3, 4.5, 5.5, 6.5, 7, 8, 9);
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
      expect(matrix.isEqualTo(matrix)).toBe(true);
    });

    it('should be equal for roughly equivalient matrices', () => {
      const matrix1 = new Matrix3x3(1, 2, 3, 4, 5, 6, 7, 8, 9);
      const matrix2 = new Matrix3x3(0.99999, 2.000001, 2.999999, 4.00001, 4.99999, 6.00001, 6.99999, 8.00001, 8.99999);
      expect(matrix1.isEqualTo(matrix2)).toBe(true);
    });

    it('should not equal for different matrices', () => {
      const matrix1 = new Matrix3x3(1, 2, 3, 4, 5, 6, 7, 8, 9);
      const matrix2 = new Matrix3x3(10, 20, 30, 40, 50, 60, 70, 80, 90);
      expect(matrix1.isEqualTo(matrix2)).toBe(false);
    });
  });
});

describe('Matrix4x4', () => {
  describe('ctor()', () => {
    it('should store the rows and columns', () => {
      const matrix = new Matrix4x4(1, 2, 3, 4, 5.5, 6.5, 7.5, 8.5, 9, 10, 11, 12, 13.5, 14.5, 15.5, 16.5);
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
      expect(matrix.isEqualTo(matrix)).toBe(true);
    });

    it('should be equal for roughly equivalient matrices', () => {
      const matrix1 = new Matrix4x4(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
      const matrix2 = new Matrix4x4(
        0.99999,
        2.000001,
        2.999999,
        4.00001,
        4.99999,
        6.00001,
        6.99999,
        8.00001,
        8.99999,
        10.00001,
        10.99999,
        12.00001,
        12.99999,
        14.00001,
        14.99999,
        16.00001,
      );
      expect(matrix1.isEqualTo(matrix2)).toBe(true);
    });

    it('should not equal for different matrices', () => {
      const matrix1 = new Matrix4x4(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
      const matrix2 = new Matrix4x4(10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150, 160);
      expect(matrix1.isEqualTo(matrix2)).toBe(false);
    });
  });
});
