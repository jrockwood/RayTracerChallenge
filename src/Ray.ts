import { Matrix4x4 } from './Matrices';
import { Point, Vector } from './PointVector';

export class Ray {
  public readonly origin: Point;
  public readonly direction: Vector;

  public constructor(origin: Point, direction: Vector) {
    this.origin = origin;
    this.direction = direction;
  }

  public position(t: number): Point {
    return this.origin.add(this.direction.multiply(t));
  }

  public transform(matrix: Matrix4x4): Ray {
    const newOrigin = matrix.multiplyByPoint(this.origin);
    const newDirection = matrix.multiplyByVector(this.direction);
    return new Ray(newOrigin, newDirection);
  }
}
