import {
  areTuplesEqual,
  isPoint,
  isVector,
  point,
  tuple,
  tupleAdd,
  tupleDivide,
  tupleMultiply,
  tupleNegate,
  tupleSubtract,
  vector,
} from '../src/tuples';

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

describe('areTuplesEqual()', () => {
  it('should be equal with exact numbers', () => {
    const a = tuple(1, 2, 3, 4);
    const b = tuple(1, 2, 3, 4);
    expect(areTuplesEqual(a, b)).toBe(true);
  });

  it('should equal if the numbers are within an epsilon', () => {
    const a = tuple(1, 2, 3, 4);
    const b = tuple(1.000009, 1.999999, 3.000001, 3.999999);
    expect(areTuplesEqual(a, b)).toBe(true);
  });

  it('should not equal if the numbers do not match', () => {
    const a = tuple(1, 2, 3, 4);
    const b = tuple(4, 3, 2, 1);
    expect(areTuplesEqual(a, b)).toBe(false);
  });
});

describe('tupleAdd()', () => {
  it('should add two tuples', () => {
    const a = tuple(3, -2, 5, 1);
    const b = tuple(-2, 3, 1, 0);
    expect(tupleAdd(a, b)).toEqual(tuple(1, 1, 6, 1));
  });
});

describe('tupleSubtract()', () => {
  it('should subtract two points', () => {
    const p1 = point(3, 2, 1);
    const p2 = point(5, 6, 7);
    expect(tupleSubtract(p1, p2)).toEqual(vector(-2, -4, -6));
  });

  it('should subtract a vector from a point', () => {
    const p = point(3, 2, 1);
    const v = vector(5, 6, 7);
    expect(tupleSubtract(p, v)).toEqual(point(-2, -4, -6));
  });

  it('should subtract two vectors', () => {
    const v1 = vector(3, 2, 1);
    const v2 = vector(5, 6, 7);
    expect(tupleSubtract(v1, v2)).toEqual(vector(-2, -4, -6));
  });

  it('should subtract a vector from the zero vector', () => {
    const zero = vector(0, 0, 0);
    const v = vector(1, -2, 3);
    expect(tupleSubtract(zero, v)).toEqual(vector(-1, 2, -3));
  });
});

describe('tupleNegate()', () => {
  it('should negate the tuple', () => {
    const a = tuple(1, -2, 3, -4);
    expect(tupleNegate(a)).toEqual(tuple(-1, 2, -3, 4));
  });
});

describe('tupleMultiply()', () => {
  it('mutliply a tuple by a scalar', () => {
    const a = tuple(1, -2, 3, -4);
    expect(tupleMultiply(a, 3.5)).toEqual(tuple(3.5, -7, 10.5, -14));
  });

  it('should multiply a tuple by a fraction', () => {
    const a = tuple(1, -2, 3, -4);
    expect(tupleMultiply(a, 0.5)).toEqual(tuple(0.5, -1, 1.5, -2));
  });
});

describe('tupleDivide()', () => {
  it('should divide a tuple by a scalar', () => {
    const a = tuple(1, -2, 3, -4);
    expect(tupleDivide(a, 2)).toEqual(tuple(0.5, -1, 1.5, -2));
  });
});
