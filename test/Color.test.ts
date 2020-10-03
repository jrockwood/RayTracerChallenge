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
});
