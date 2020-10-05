import { Point, Vector } from '../src/PointVector';

describe('Point', () => {
  it('ctor should store x, y, and z', () => {
    const point = new Point(4.3, -4.2, 3.1);
    expect(point.x).toBe(4.3);
    expect(point.y).toBe(-4.2);
    expect(point.z).toBe(3.1);
  });

  describe('isEqualTo()', () => {
    it('should be equal with exact numbers', () => {
      const a = new Point(1, 2, 3);
      const b = new Point(1, 2, 3);
      expect(a.isEqualTo(b)).toBeTrue();
    });

    it('should equal if the numbers are within an epsilon', () => {
      const a = new Point(1, 2, 3);
      const b = new Point(1.000009, 1.999999, 3.000001);
      expect(a.isEqualTo(b)).toBeTrue();
    });

    it('should not equal if the numbers do not match', () => {
      const a = new Point(1, 2, 3);
      const b = new Point(4, 3, 2);
      expect(a.isEqualTo(b)).toBeFalse();
    });
  });

  describe('toTuple()', () => {
    it('should return with w=1', () => {
      const p = new Point(7, 8, 9);
      expect(p.toTuple()).toEqual({ x: 7, y: 8, z: 9, w: 1 });
    });
  });

  describe('add()', () => {
    it('should add a vector', () => {
      const p = new Point(3, -2, 5);
      const v = new Vector(-2, 3, 1);
      expect(p.add(v)).toEqual(new Point(1, 1, 6));
    });
  });

  describe('subtract()', () => {
    it('should subtract two points', () => {
      const p1 = new Point(3, 2, 1);
      const p2 = new Point(5, 6, 7);
      expect(p1.subtract(p2)).toEqual(new Vector(-2, -4, -6));
    });

    it('should subtract a vector from a point', () => {
      const p = new Point(3, 2, 1);
      const v = new Vector(5, 6, 7);
      expect(p.subtractVector(v)).toEqual(new Point(-2, -4, -6));
    });
  });
});

describe('Vector', () => {
  it('ctor should store x, y, and z', () => {
    const vector = new Vector(4.3, -4.2, 3.1);
    expect(vector.x).toBe(4.3);
    expect(vector.y).toBe(-4.2);
    expect(vector.z).toBe(3.1);
  });

  describe('isEqualTo()', () => {
    it('should be equal with exact numbers', () => {
      const a = new Vector(1, 2, 3);
      const b = new Vector(1, 2, 3);
      expect(a.isEqualTo(b)).toBeTrue();
    });

    it('should equal if the numbers are within an epsilon', () => {
      const a = new Vector(1, 2, 3);
      const b = new Vector(1.000009, 1.999999, 3.000001);
      expect(a.isEqualTo(b)).toBeTrue();
    });

    it('should not equal if the numbers do not match', () => {
      const a = new Vector(1, 2, 3);
      const b = new Vector(4, 3, 2);
      expect(a.isEqualTo(b)).toBeFalse();
    });
  });

  describe('toTuple()', () => {
    it('should return with w=0', () => {
      const v = new Vector(7, 8, 9);
      expect(v.toTuple()).toEqual({ x: 7, y: 8, z: 9, w: 0 });
    });
  });

  describe('add()', () => {
    it('should add a vector', () => {
      const v1 = new Vector(3, -2, 5);
      const v2 = new Vector(-2, 3, 1);
      expect(v1.add(v2)).toEqual(new Vector(1, 1, 6));
    });

    it('should add a point', () => {
      const v = new Vector(3, -2, 5);
      const p = new Point(-2, 3, 1);
      expect(v.addPoint(p)).toEqual(new Point(1, 1, 6));
    });
  });

  describe('subtract()', () => {
    it('should subtract two vectors', () => {
      const v1 = new Vector(3, 2, 1);
      const v2 = new Vector(5, 6, 7);
      expect(v1.subtract(v2)).toEqual(new Vector(-2, -4, -6));
    });

    it('should subtract a vector from the zero vector', () => {
      const zero = new Vector(0, 0, 0);
      const v = new Vector(1, -2, 3);
      expect(zero.subtract(v)).toEqual(new Vector(-1, 2, -3));
    });
  });

  describe('negate()', () => {
    it('should negate the vector', () => {
      const v = new Vector(1, -2, 3);
      expect(v.negate()).toEqual(new Vector(-1, 2, -3));
    });
  });

  describe('multiply()', () => {
    it('mutliply the vector by a scalar', () => {
      const v = new Vector(1, -2, 3);
      expect(v.multiply(3.5)).toEqual(new Vector(3.5, -7, 10.5));
    });

    it('should multiply a tuple by a fraction', () => {
      const v = new Vector(1, -2, 3);
      expect(v.multiply(0.5)).toEqual(new Vector(0.5, -1, 1.5));
    });
  });

  describe('divide()', () => {
    it('should divide a vector by a scalar', () => {
      const v = new Vector(1, -2, 3);
      expect(v.divide(2)).toEqual(new Vector(0.5, -1, 1.5));
    });
  });

  describe('magnitude()', () => {
    it('should return the length of a unit vector', () => {
      let v = new Vector(1, 0, 0);
      expect(v.magnitude()).toBe(1);

      v = new Vector(0, 1, 0);
      expect(v.magnitude()).toBe(1);

      v = new Vector(0, 0, 1);
      expect(v.magnitude()).toBe(1);
    });

    it('should return the length of a non-unit vector', () => {
      let v = new Vector(1, 2, 3);
      expect(v.magnitude()).toBe(Math.sqrt(1 + 4 + 9));

      v = new Vector(-1, -2, -3);
      expect(v.magnitude()).toBe(Math.sqrt(1 + 4 + 9));
    });
  });

  describe('normalize()', () => {
    it('should not change unit vectors', () => {
      let v = new Vector(1, 0, 0);
      expect(v.normalize()).toEqual(v);

      v = new Vector(0, 1, 0);
      expect(v.normalize()).toEqual(v);

      v = new Vector(0, 0, 1);
      expect(v.normalize()).toEqual(v);
    });

    it('should normalize non-unit vectors', () => {
      let v = new Vector(4, 0, 0);
      expect(v.normalize()).toEqual(new Vector(1, 0, 0));

      v = new Vector(1, 2, 3);
      const length = Math.sqrt(1 + 4 + 9);
      expect(v.normalize()).toEqual(new Vector(1 / length, 2 / length, 3 / length));
    });

    it('should return 1 for the magnitude of a normalized vector', () => {
      const v = new Vector(1, 2, 3);
      const normalized = v.normalize();
      expect(normalized.magnitude()).toBe(1);
    });
  });

  describe('dot()', () => {
    it('should compute the dot product of two vectors', () => {
      const a = new Vector(1, 2, 3);
      const b = new Vector(2, 3, 4);
      expect(a.dot(b)).toBe(20);
    });
  });

  describe('cross()', () => {
    it('should compute the cross product of two vectors', () => {
      const a = new Vector(1, 2, 3);
      const b = new Vector(2, 3, 4);
      expect(a.cross(b)).toEqual(new Vector(-1, 2, -1));
      expect(b.cross(a)).toEqual(new Vector(1, -2, 1));
    });
  });

  describe('reflect()', () => {
    it('should reflect a vector approaching at 45 degrees', () => {
      const vector = new Vector(1, -1, 0);
      const normal = new Vector(0, 1, 0);
      const reflection = vector.reflect(normal);
      expect(reflection).toEqual(new Vector(1, 1, 0));
    });

    it('should reflect a vector off a slanted surface', () => {
      const vector = new Vector(0, -1, 0);
      const normal = new Vector(Math.SQRT2 / 2, Math.SQRT2 / 2, 0);
      const reflection = vector.reflect(normal);
      expect(reflection.isEqualTo(new Vector(1, 0, 0))).toBeTrue();
    });
  });
});
