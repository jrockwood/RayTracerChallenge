import { Canvas } from '../src/Canvas';
import { Color } from '../src/Color';

describe('Canvas', () => {
  describe('ctor()', () => {
    it('should store the width and height', () => {
      const canvas = new Canvas(10, 20);
      expect(canvas.width).toBe(10);
      expect(canvas.height).toBe(20);
    });

    it('should default to all black pixels', () => {
      const canvas = new Canvas(4, 4);
      for (let x = 0; x < 4; x++) {
        for (let y = 0; y < 4; y++) {
          expect(canvas.getPixel(x, y)).toEqual(Color.Black);
        }
      }
    });
  });

  describe('setPixel()', () => {
    it('should set the color of the pixel', () => {
      const canvas = new Canvas(10, 20);
      canvas.setPixel(2, 3, Color.Red);
      expect(canvas.getPixel(2, 3)).toEqual(Color.Red);
    });

    it('should throw if the index is out of bounds', () => {
      const canvas = new Canvas(10, 20);
      expect(() => canvas.setPixel(-1, 0, Color.White)).toThrow();
      expect(() => canvas.setPixel(11, 0, Color.White)).toThrow();
      expect(() => canvas.setPixel(10, 0, Color.White)).toThrow();
      expect(() => canvas.setPixel(0, -1, Color.White)).toThrow();
      expect(() => canvas.setPixel(0, 21, Color.White)).toThrow();
      expect(() => canvas.setPixel(0, 20, Color.White)).toThrow();
    });
  });

  describe('toPpm()', () => {
    it('should create a valid ppm header', () => {
      const canvas = new Canvas(5, 3);
      expect(canvas.toPpm().split('\n', 3)).toEqual(['P3', '5 3', '255']);
    });

    it('should add the pixel data', () => {
      const canvas = new Canvas(3, 1);
      canvas.setPixel(0, 0, Color.Red);
      canvas.setPixel(1, 0, Color.Green);
      canvas.setPixel(2, 0, Color.Blue);
      expect(canvas.toPpm().split('\n')).toEqual(['P3', '3 1', '255', '255 0 0 0 255 0 0 0 255', '']);
    });

    it('should scale and clamp the pixel data', () => {
      const canvas = new Canvas(3, 1);
      canvas.setPixel(0, 0, new Color(1.5, 0, 0));
      canvas.setPixel(1, 0, new Color(0, 0.5, 0));
      canvas.setPixel(2, 0, new Color(-0.5, 0, 1));

      const dataLines = canvas.toPpm().split('\n');
      expect(dataLines).toEqual(['P3', '3 1', '255', '255 0 0 0 128 0 0 0 255', '']);
    });

    it('should split color data lines to shorter than 70 characters', () => {
      const canvas = new Canvas(10, 2);
      const color = new Color(1, 0.8, 0.6);
      canvas.clear(color);
      const dataLines = canvas.toPpm().split('\n');
      expect(dataLines).toEqual([
        'P3',
        '10 2',
        '255',
        '255 204 153 255 204 153 255 204 153 255 204 153 255 204 153 255 204',
        '153 255 204 153 255 204 153 255 204 153 255 204 153 255 204 153 255',
        '204 153 255 204 153 255 204 153 255 204 153 255 204 153 255 204 153',
        '255 204 153 255 204 153 255 204 153',
        '',
      ]);
    });

    it('should be terminated by a newline character', () => {
      const canvas = new Canvas(10, 2);
      const color = new Color(1, 0.8, 0.6);
      canvas.clear(color);
      const ppm = canvas.toPpm();
      expect(ppm.charAt(ppm.length - 1)).toBe('\n');
    });
  });
});
