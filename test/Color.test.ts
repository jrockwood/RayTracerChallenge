import { Color } from '../src/Color';

describe('Color', () => {
  describe('constructor', () => {
    it('should store the red, green, and blue components', () => {
      const color = new Color(-0.5, 0.4, 1.7);
      expect(color.red).toBe(-0.5);
      expect(color.green).toBe(0.4);
      expect(color.blue).toBe(1.7);
    });
  });

  describe('add()', () => {
    it('should add two colors', () => {
      const c1 = new Color(0.9, 0.6, 0.75);
      const c2 = new Color(0.7, 0.1, 0.25);
      expect(c1.add(c2)).toEqual(new Color(1.6, 0.7, 1.0));
    });
  });

  describe('subtract()', () => {
    it('should subtract two colors', () => {
      const c1 = new Color(0.9, 0.6, 0.75);
      const c2 = new Color(0.7, 0.1, 0.25);
      expect(c1.subtract(c2).isEqualTo(new Color(0.2, 0.5, 0.5))).toBe(true);
    });
  });

  describe('multiply()', () => {
    it('should multiply a color by a scalar', () => {
      const c = new Color(0.2, 0.3, 0.4);
      expect(c.multiply(2)).toEqual(new Color(0.4, 0.6, 0.8));
    });

    it('should multiple two colors', () => {
      const c1 = new Color(1, 0.2, 0.4);
      const c2 = new Color(0.9, 1, 0.1);
      expect(c1.multiply(c2).isEqualTo(new Color(0.9, 0.2, 0.04))).toBe(true);
    });
  });
});
