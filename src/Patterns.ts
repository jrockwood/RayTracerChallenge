import { Color } from './Color';
import { Matrix4x4 } from './Matrices';
import { Point } from './PointVector';
import { Shape } from './Shapes';

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Pattern

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

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// StripePattern

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

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// GradientPattern

export class GradientPattern extends Pattern {
  public readonly a: Color;
  public readonly b: Color;

  public constructor(a: Color, b: Color, transform?: Matrix4x4) {
    super(transform);

    this.a = a;
    this.b = b;
  }

  public colorAt(point: Point): Color {
    const distance = this.b.subtract(this.a);
    const fraction = point.x - Math.floor(point.x);
    const color = this.a.add(distance.multiply(fraction));
    return color;
  }

  public withTransform(value: Matrix4x4): Pattern {
    return new GradientPattern(this.a, this.b, value);
  }
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// RingPattern

export class RingPattern extends Pattern {
  public readonly a: Color;
  public readonly b: Color;

  public constructor(a: Color, b: Color, transform?: Matrix4x4) {
    super(transform);

    this.a = a;
    this.b = b;
  }

  public colorAt(point: Point): Color {
    if (Math.sqrt(point.x * point.x + point.z * point.z) % 2 === 0) {
      return this.a;
    }

    return this.b;
  }

  public withTransform(value: Matrix4x4): Pattern {
    return new RingPattern(this.a, this.b, value);
  }
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// CheckerPattern

export class CheckerPattern extends Pattern {
  public readonly a: Color;
  public readonly b: Color;

  public constructor(a: Color, b: Color, transform?: Matrix4x4) {
    super(transform);

    this.a = a;
    this.b = b;
  }

  public colorAt(point: Point): Color {
    if ((Math.floor(point.x) + Math.floor(point.y) + Math.floor(point.z)) % 2 === 0) {
      return this.a;
    }

    return this.b;
  }

  public withTransform(value: Matrix4x4): Pattern {
    return new CheckerPattern(this.a, this.b, value);
  }
}
