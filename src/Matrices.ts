import { floatEqual } from './Math';
import { Point, Tuple, Vector } from './PointVector';

export class Matrix2x2 {
  public constructor(
    public readonly m00: number,
    public readonly m01: number,
    public readonly m10: number,
    public readonly m11: number,
  ) {}

  public isEqualTo(other: Matrix2x2): boolean {
    return (
      floatEqual(this.m00, other.m00) &&
      floatEqual(this.m01, other.m01) &&
      floatEqual(this.m10, other.m10) &&
      floatEqual(this.m11, other.m11)
    );
  }
}

export class Matrix3x3 {
  public constructor(
    public readonly m00: number,
    public readonly m01: number,
    public readonly m02: number,
    public readonly m10: number,
    public readonly m11: number,
    public readonly m12: number,
    public readonly m20: number,
    public readonly m21: number,
    public readonly m22: number,
  ) {}

  public isEqualTo(other: Matrix3x3): boolean {
    return (
      floatEqual(this.m00, other.m00) &&
      floatEqual(this.m01, other.m01) &&
      floatEqual(this.m02, other.m02) &&
      floatEqual(this.m10, other.m10) &&
      floatEqual(this.m11, other.m11) &&
      floatEqual(this.m12, other.m12) &&
      floatEqual(this.m20, other.m20) &&
      floatEqual(this.m21, other.m21) &&
      floatEqual(this.m22, other.m22)
    );
  }
}

export class Matrix4x4 {
  // prettier-ignore
  public static identity: Matrix4x4 = new Matrix4x4(
    1, 0, 0, 0,
    0, 1, 0, 0,
    0, 0, 1, 0,
    0, 0, 0, 1);

  public constructor(
    public readonly m00: number,
    public readonly m01: number,
    public readonly m02: number,
    public readonly m03: number,
    public readonly m10: number,
    public readonly m11: number,
    public readonly m12: number,
    public readonly m13: number,
    public readonly m20: number,
    public readonly m21: number,
    public readonly m22: number,
    public readonly m23: number,
    public readonly m30: number,
    public readonly m31: number,
    public readonly m32: number,
    public readonly m33: number,
  ) {}

  public isEqualTo(other: Matrix4x4): boolean {
    return (
      floatEqual(this.m00, other.m00) &&
      floatEqual(this.m01, other.m01) &&
      floatEqual(this.m02, other.m02) &&
      floatEqual(this.m03, other.m03) &&
      floatEqual(this.m10, other.m10) &&
      floatEqual(this.m11, other.m11) &&
      floatEqual(this.m12, other.m12) &&
      floatEqual(this.m13, other.m13) &&
      floatEqual(this.m20, other.m20) &&
      floatEqual(this.m21, other.m21) &&
      floatEqual(this.m22, other.m22) &&
      floatEqual(this.m23, other.m23) &&
      floatEqual(this.m30, other.m30) &&
      floatEqual(this.m31, other.m31) &&
      floatEqual(this.m32, other.m32) &&
      floatEqual(this.m33, other.m33)
    );
  }

  public multiply(matrix: Matrix4x4): Matrix4x4 {
    return new Matrix4x4(
      // First row
      this.m00 * matrix.m00 + this.m01 * matrix.m10 + this.m02 * matrix.m20 + this.m03 * matrix.m30,
      this.m00 * matrix.m01 + this.m01 * matrix.m11 + this.m02 * matrix.m21 + this.m03 * matrix.m31,
      this.m00 * matrix.m02 + this.m01 * matrix.m12 + this.m02 * matrix.m22 + this.m03 * matrix.m32,
      this.m00 * matrix.m03 + this.m01 * matrix.m13 + this.m02 * matrix.m23 + this.m03 * matrix.m33,

      // Second row
      this.m10 * matrix.m00 + this.m11 * matrix.m10 + this.m12 * matrix.m20 + this.m13 * matrix.m30,
      this.m10 * matrix.m01 + this.m11 * matrix.m11 + this.m12 * matrix.m21 + this.m13 * matrix.m31,
      this.m10 * matrix.m02 + this.m11 * matrix.m12 + this.m12 * matrix.m22 + this.m13 * matrix.m32,
      this.m10 * matrix.m03 + this.m11 * matrix.m13 + this.m12 * matrix.m23 + this.m13 * matrix.m33,

      // Third row
      this.m20 * matrix.m00 + this.m21 * matrix.m10 + this.m22 * matrix.m20 + this.m23 * matrix.m30,
      this.m20 * matrix.m01 + this.m21 * matrix.m11 + this.m22 * matrix.m21 + this.m23 * matrix.m31,
      this.m20 * matrix.m02 + this.m21 * matrix.m12 + this.m22 * matrix.m22 + this.m23 * matrix.m32,
      this.m20 * matrix.m03 + this.m21 * matrix.m13 + this.m22 * matrix.m23 + this.m23 * matrix.m33,

      // Fourth row
      this.m30 * matrix.m00 + this.m31 * matrix.m10 + this.m32 * matrix.m20 + this.m33 * matrix.m30,
      this.m30 * matrix.m01 + this.m31 * matrix.m11 + this.m32 * matrix.m21 + this.m33 * matrix.m31,
      this.m30 * matrix.m02 + this.m31 * matrix.m12 + this.m32 * matrix.m22 + this.m33 * matrix.m32,
      this.m30 * matrix.m03 + this.m31 * matrix.m13 + this.m32 * matrix.m23 + this.m33 * matrix.m33,
    );
  }

  public multiplyByPoint(point: Point): Point {
    const tuple = this.multiplyTuple(point.toTuple());
    return new Point(tuple.x, tuple.y, tuple.z);
  }

  public multiplyByVector(vector: Vector): Vector {
    const tuple = this.multiplyTuple(vector.toTuple());
    return new Vector(tuple.x, tuple.y, tuple.z);
  }

  private multiplyTuple(tuple: Tuple): Tuple {
    const x = this.m00 * tuple.x + this.m01 * tuple.y + this.m02 * tuple.z + this.m03 * tuple.w;
    const y = this.m10 * tuple.x + this.m11 * tuple.y + this.m12 * tuple.z + this.m13 * tuple.w;
    const z = this.m20 * tuple.x + this.m21 * tuple.y + this.m22 * tuple.z + this.m23 * tuple.w;
    const w = this.m30 * tuple.x + this.m31 * tuple.y + this.m23 * tuple.z + this.m33 * tuple.w;
    return { x, y, z, w };
  }
}
