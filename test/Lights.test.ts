import { Color } from '../src/Color';
import { PointLight } from '../src/Lights';
import { Point } from '../src/PointVector';

describe('PointLight', () => {
  describe('ctor()', () => {
    it('should store a position and intensity', () => {
      const intensity = new Color(1, 1, 1);
      const position = Point.zero;
      const light = new PointLight(position, intensity);
      expect(light.position).toBe(position);
      expect(light.intensity).toBe(intensity);
    });
  });
});
