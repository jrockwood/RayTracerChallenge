import { Point, Vector } from '../src/tuples';

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
      expect(a.isEqualTo(b)).toBe(true);
    });

    it('should equal if the numbers are within an epsilon', () => {
      const a = new Point(1, 2, 3);
      const b = new Point(1.000009, 1.999999, 3.000001);
      expect(a.isEqualTo(b)).toBe(true);
    });

    it('should not equal if the numbers do not match', () => {
      const a = new Point(1, 2, 3);
      const b = new Point(4, 3, 2);
      expect(a.isEqualTo(b)).toBe(false);
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
      expect(a.isEqualTo(b)).toBe(true);
    });

    it('should equal if the numbers are within an epsilon', () => {
      const a = new Vector(1, 2, 3);
      const b = new Vector(1.000009, 1.999999, 3.000001);
      expect(a.isEqualTo(b)).toBe(true);
    });

    it('should not equal if the numbers do not match', () => {
      const a = new Vector(1, 2, 3);
      const b = new Vector(4, 3, 2);
      expect(a.isEqualTo(b)).toBe(false);
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
});
