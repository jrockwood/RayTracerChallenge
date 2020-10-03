import { floatEqual } from './Math';

export class Matrix2x2 {
  public static identiry: Matrix2x2 = new Matrix2x2(0, 0, 0, 0);

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
  public static identiry: Matrix3x3 = new Matrix3x3(0, 0, 0, 0, 0, 0, 0, 0, 0);

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
  public static identiry: Matrix4x4 = new Matrix4x4(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

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
}
