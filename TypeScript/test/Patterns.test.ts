import { Color } from '../src/Color';
import { Matrix4x4 } from '../src/Matrices';
import { CheckerPattern, GradientPattern, Pattern, RingPattern, StripePattern } from '../src/Patterns';
import { Point } from '../src/PointVector';
import { Sphere } from '../src/Shapes';

export class TestPattern extends Pattern {
  public constructor(transform?: Matrix4x4) {
    super(transform);
  }

  public colorAt(point: Point): Color {
    return new Color(point.x, point.y, point.z);
  }

  public withTransform(value: Matrix4x4): Pattern {
    return new TestPattern(value);
  }
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Pattern

describe('Pattern', () => {
  describe('ctor()', () => {
    it('should default the transformation matrix to the identity matrix', () => {
      const pattern = new TestPattern();
      expect(pattern.transform).toEqual(Matrix4x4.identity);
    });

    it('should store the transformation matrix', () => {
      const pattern = new TestPattern(Matrix4x4.translation(1, 2, 3));
      expect(pattern.transform).toEqual(Matrix4x4.translation(1, 2, 3));
    });
  });

  describe('colorOnShapeAt()', () => {
    it('should use the shape transformation', () => {
      const shape = new Sphere(Matrix4x4.scaling(2, 2, 2));
      const pattern = new TestPattern();
      const color = pattern.colorOnShapeAt(shape, new Point(2, 3, 4));
      expect(color).toEqual(new Color(1, 1.5, 2));
    });

    it('should use the pattern transformation', () => {
      const shape = new Sphere();
      const pattern = new TestPattern(Matrix4x4.scaling(2, 2, 2));
      const color = pattern.colorOnShapeAt(shape, new Point(2, 3, 4));
      expect(color).toEqual(new Color(1, 1.5, 2));
    });

    it('should use both the shape and pattern transformation', () => {
      const shape = new Sphere(Matrix4x4.scaling(2, 2, 2));
      const pattern = new TestPattern(Matrix4x4.translation(0.5, 1, 1.5));
      const color = pattern.colorOnShapeAt(shape, new Point(2.5, 3, 3.5));
      expect(color).toEqual(new Color(0.75, 0.5, 0.25));
    });
  });
});

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// StripePattern

describe('StripePattern', () => {
  describe('ctor()', () => {
    it('should store the two colors for the stripe', () => {
      const pattern = new StripePattern(Color.White, Color.Black);
      expect(pattern.a).toEqual(Color.White);
      expect(pattern.b).toEqual(Color.Black);
    });
  });

  describe('colorAt()', () => {
    it('should return the same color for every y coordinate', () => {
      const pattern = new StripePattern(Color.White, Color.Black);
      expect(pattern.colorAt(new Point(0, 0, 0))).toEqual(Color.White);
      expect(pattern.colorAt(new Point(0, 1, 0))).toEqual(Color.White);
      expect(pattern.colorAt(new Point(0, 2, 0))).toEqual(Color.White);
    });

    it('should return the same color for every z coordinate', () => {
      const pattern = new StripePattern(Color.White, Color.Black);
      expect(pattern.colorAt(new Point(0, 0, 0))).toEqual(Color.White);
      expect(pattern.colorAt(new Point(0, 0, 1))).toEqual(Color.White);
      expect(pattern.colorAt(new Point(0, 0, 2))).toEqual(Color.White);
    });

    it('should alternate color with x coordinates', () => {
      const pattern = new StripePattern(Color.White, Color.Black);
      expect(pattern.colorAt(new Point(0, 0, 0))).toEqual(Color.White);
      expect(pattern.colorAt(new Point(0.9, 0, 0))).toEqual(Color.White);
      expect(pattern.colorAt(new Point(1, 0, 0))).toEqual(Color.Black);
      expect(pattern.colorAt(new Point(-0.1, 0, 0))).toEqual(Color.Black);
      expect(pattern.colorAt(new Point(-1.1, 0, 0))).toEqual(Color.White);
    });
  });

  describe('colorOnShapeAt()', () => {
    it('should use the shape transform', () => {
      const shape = new Sphere(Matrix4x4.scaling(2, 2, 2));
      const pattern = new StripePattern(Color.White, Color.Black);
      const color = pattern.colorOnShapeAt(shape, new Point(1.5, 0, 0));
      expect(color).toEqual(Color.White);
    });

    it('should use the pattern transform', () => {
      const shape = new Sphere();
      const pattern = new StripePattern(Color.White, Color.Black, Matrix4x4.translation(0.5, 0, 0));
      const color = pattern.colorOnShapeAt(shape, new Point(2.5, 0, 0));
      expect(color).toEqual(Color.White);
    });
  });
});

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// GradientPattern

describe('GradientPattern', () => {
  describe('colorAt()', () => {
    it('should linearly interpolate between colors', () => {
      const pattern = new GradientPattern(Color.White, Color.Black);
      expect(pattern.colorAt(new Point(0, 0, 0))).toEqual(Color.White);
      expect(pattern.colorAt(new Point(0.25, 0, 0))).toEqual(new Color(0.75, 0.75, 0.75));
      expect(pattern.colorAt(new Point(0.5, 0, 0))).toEqual(new Color(0.5, 0.5, 0.5));
      expect(pattern.colorAt(new Point(0.75, 0, 0))).toEqual(new Color(0.25, 0.25, 0.25));
    });
  });
});

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// RingPattern

describe('RingPattern', () => {
  describe('colorAt()', () => {
    it('should extend in both x and z', () => {
      const pattern = new RingPattern(Color.White, Color.Black);
      expect(pattern.colorAt(new Point(0, 0, 0))).toEqual(Color.White);
      expect(pattern.colorAt(new Point(1, 0, 0))).toEqual(Color.Black);
      expect(pattern.colorAt(new Point(0, 0, 1))).toEqual(Color.Black);

      // 0.708 is just slightly more than sqrt(2)/2
      expect(pattern.colorAt(new Point(0.708, 0, 0.708))).toEqual(Color.Black);
    });
  });
});

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// CheckerPattern

describe('CheckerPattern', () => {
  describe('colorAt()', () => {
    it('should repeat in x', () => {
      const pattern = new CheckerPattern(Color.White, Color.Black);
      expect(pattern.colorAt(new Point(0, 0, 0))).toEqual(Color.White);
      expect(pattern.colorAt(new Point(0.99, 0, 0))).toEqual(Color.White);
      expect(pattern.colorAt(new Point(1.01, 0, 0))).toEqual(Color.Black);
    });

    it('should repeat in y', () => {
      const pattern = new CheckerPattern(Color.White, Color.Black);
      expect(pattern.colorAt(new Point(0, 0, 0))).toEqual(Color.White);
      expect(pattern.colorAt(new Point(0, 0.99, 0))).toEqual(Color.White);
      expect(pattern.colorAt(new Point(0, 1.01, 0))).toEqual(Color.Black);
    });

    it('should repeat in z', () => {
      const pattern = new CheckerPattern(Color.White, Color.Black);
      expect(pattern.colorAt(new Point(0, 0, 0))).toEqual(Color.White);
      expect(pattern.colorAt(new Point(0, 0, 0.99))).toEqual(Color.White);
      expect(pattern.colorAt(new Point(0, 0, 1.01))).toEqual(Color.Black);
    });
  });
});
