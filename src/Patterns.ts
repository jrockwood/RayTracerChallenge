import { Color } from './Color';
import { Matrix4x4 } from './Matrices';
import { Point } from './PointVector';
import { Shape } from './Shapes';

export abstract class Pattern {
  public readonly transform: Matrix4x4;

  protected constructor(transform: Matrix4x4 = Matrix4x4.identity) {
    this.transform = transform;
  }

  public abstract colorAt(point: Point): Color;
  public colorOnShapeAt(shape: Shape, worldPoint: Point): Color {
    const shapePoint = shape.transform.inverse().multiplyByPoint(worldPoint);
    const patternPoint = this.transform.inverse().multiplyByPoint(shapePoint);
    const color = this.colorAt(patternPoint);
    return color;
  }

  public abstract withTransform(value: Matrix4x4): Pattern;
}

export class StripePattern extends Pattern {
  public readonly a: Color;
  public readonly b: Color;

  public constructor(a: Color, b: Color, transform?: Matrix4x4) {
    super(transform);

    this.a = a;
    this.b = b;
  }

  public colorAt(point: Point): Color {
    return Math.floor(point.x) % 2 === 0 ? this.a : this.b;
  }

  public withTransform(value: Matrix4x4): Pattern {
    return new StripePattern(this.a, this.b, value);
  }
}
