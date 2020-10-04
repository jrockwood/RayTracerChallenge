import { clamp, degressToRadians, floatEqual } from '../src/Math';

describe('floatEqual()', () => {
  it('should return true for exact equality', () => {
    expect(floatEqual(0.1234, 0.1234)).toBeTrue();
  });

  it('should return true for approximate equality', () => {
    expect(floatEqual(10, 9.99999)).toBeTrue();
  });
});

describe('clamp()', () => {
  it('should return the value if it is within range', () => {
    expect(clamp(1, 0, 10)).toBe(1);
  });

  it('should clamp if the value is less than the minimum', () => {
    expect(clamp(-1, 0, 10)).toBe(0);
  });

  it('should clamp if the value is greater than the maximum', () => {
    expect(clamp(11, 0, 10)).toBe(10);
  });

  it('should throw if min is greater than max', () => {
    expect(() => clamp(1, 10, 0)).toThrow();
  });

  it('should allow the value, min, and max to be the same', () => {
    expect(clamp(1, 1, 1)).toBe(1);
  });
});

describe('degressToRadians()', () => {
  it('should convert degress to radians', () => {
    expect(degressToRadians(0)).toBe(0);
    expect(degressToRadians(45)).toBe(Math.PI / 4);
    expect(degressToRadians(90)).toBe(Math.PI / 2);
    expect(degressToRadians(135)).toBe(Math.PI * (3 / 4));
    expect(degressToRadians(180)).toBe(Math.PI);
    expect(degressToRadians(270)).toBe(Math.PI + Math.PI / 2);
    expect(degressToRadians(360)).toBe(Math.PI * 2);
  });
});
