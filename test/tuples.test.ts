import { areTuplesEqual, isPoint, isVector, point, tuple, vector } from '../src/tuples';

describe('tuple()', () => {
  it('should store x, y, z, and w', () => {
    const point = tuple(4.3, -4.2, 3.1, 1.0);
    expect(point.x).toBe(4.3);
    expect(point.y).toBe(-4.2);
    expect(point.z).toBe(3.1);
    expect(point.w).toBe(1.0);
  });

  describe('with w=1.0', () => {
    const point = tuple(4.3, -4.2, 3.1, 1.0);

    it('should be a point', () => {
      expect(isPoint(point)).toBe(true);
    });

    it('should not be a vector', () => {
      expect(isVector(point)).toBe(false);
    });
  });

  describe('with w=0', () => {
    const vector = tuple(4.3, -4.2, 3.1, 0.0);

    it('should be a vector', () => {
      expect(isVector(vector)).toBe(true);
    });

    it('should not be a point', () => {
      expect(isPoint(vector)).toBe(false);
    });
  });
});

describe('point()', () => {
  it('creates tuples with w=1', () => {
    expect(point(4, -4, 3)).toEqual(tuple(4, -4, 3, 1));
  });
});

describe('vector()', () => {
  it('creates tuples with w=0', () => {
    expect(vector(4, -4, 3)).toEqual(tuple(4, -4, 3, 0));
  });
});
