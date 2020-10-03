import { floatEqual } from './Math';

export class Point {
  public readonly x: number;
  public readonly y: number;
  public readonly z: number;

  public constructor(x: number, y: number, z: number) {
    this.x = x;
    this.y = y;
    this.z = z;
  }

  public isEqualTo(other: Point): boolean {
    return floatEqual(this.x, other.x) && floatEqual(this.y, other.y) && floatEqual(this.z, other.z);
  }

  public add(vector: Vector): Point {
    return new Point(this.x + vector.x, this.y + vector.y, this.z + vector.z);
  }

  public subtract(point: Point): Vector {
    return new Vector(this.x - point.x, this.y - point.y, this.z - point.z);
  }

  public subtractVector(vector: Vector): Point {
    return new Point(this.x - vector.x, this.y - vector.y, this.z - vector.z);
  }
}

export class Vector {
  public readonly x: number;
  public readonly y: number;
  public readonly z: number;

  public constructor(x: number, y: number, z: number) {
    this.x = x;
    this.y = y;
    this.z = z;
  }

  public isEqualTo(other: Vector): boolean {
    return floatEqual(this.x, other.x) && floatEqual(this.y, other.y) && floatEqual(this.z, other.z);
  }

  public add(vector: Vector): Vector {
    return new Vector(this.x + vector.x, this.y + vector.y, this.z + vector.z);
  }

  public addPoint(point: Point): Point {
    return new Point(this.x + point.x, this.y + point.y, this.z + point.z);
  }

  public subtract(vector: Vector): Vector {
    return new Vector(this.x - vector.x, this.y - vector.y, this.z - vector.z);
  }

  public negate(): Vector {
    return new Vector(-this.x, -this.y, -this.z);
  }

  public multiply(scalar: number): Vector {
    return new Vector(this.x * scalar, this.y * scalar, this.z * scalar);
  }

  public divide(scalar: number): Vector {
    return new Vector(this.x / scalar, this.y / scalar, this.z / scalar);
  }

  public magnitude(): number {
    return Math.sqrt(this.x * this.x + this.y * this.y + this.z * this.z);
  }

  public normalize(): Vector {
    return this.divide(this.magnitude());
  }

  public dot(vector: Vector): number {
    return this.x * vector.x + this.y * vector.y + this.z * vector.z;
  }

  public cross(vector: Vector): Vector {
    return new Vector(
      this.y * vector.z - this.z * vector.y,
      this.z * vector.x - this.x * vector.z,
      this.x * vector.y - this.y * vector.x,
    );
  }
}
