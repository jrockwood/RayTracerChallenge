const Epsilon = 0.00001;

function floatEqual(a: number, b: number): boolean {
  return Math.abs(a - b) < Epsilon;
}
export class Point {
  public constructor(public readonly x: number, public readonly y: number, public readonly z: number) {}

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
  public constructor(public readonly x: number, public readonly y: number, public readonly z: number) {}

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
}
