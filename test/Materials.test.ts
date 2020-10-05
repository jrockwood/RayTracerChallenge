import { Color } from '../src/Color';
import { PointLight } from '../src/Lights';
import { Material } from '../src/Materials';
import { Point, Vector } from '../src/PointVector';

describe('Material', () => {
  describe('ctor()', () => {
    it('should use default values for the ambient, diffuse, and specular values', () => {
      const material = new Material();
      expect(material.color).toEqual(Color.White);
      expect(material.ambient).toBe(Material.defaultAmbient);
      expect(material.diffuse).toBe(Material.defaultDiffuse);
      expect(material.specular).toBe(Material.defaultSpecular);
      expect(material.shininess).toBe(Material.defaultShininess);
    });

    it('should throw for negative values', () => {
      expect(() => new Material(undefined, -1)).toThrow();
      expect(() => new Material(undefined, undefined, -1)).toThrow();
      expect(() => new Material(undefined, undefined, undefined, -1)).toThrow();
      expect(() => new Material(undefined, undefined, undefined, undefined, -1)).toThrow();
    });
  });

  describe('lighting()', () => {
    describe('should calculate the color of the point', () => {
      const material = new Material();
      const position = Point.zero;

      it('with the eye between the light and the surface', () => {
        const eye = new Vector(0, 0, -1);
        const normal = new Vector(0, 0, -1);
        const light = new PointLight(new Point(0, 0, -10), Color.White);
        const result = material.lighting(light, position, eye, normal);
        expect(result).toEqual(new Color(1.9, 1.9, 1.9));
      });

      it('with the eye between the light and surface and the eye offset 45 degrees', () => {
        const eye = new Vector(0, Math.SQRT2 / 2, -Math.SQRT2 / 2);
        const normal = new Vector(0, 0, -1);
        const light = new PointLight(new Point(0, 0, -10), Color.White);
        const result = material.lighting(light, position, eye, normal);
        expect(result).toEqual(new Color(1.0, 1.0, 1.0));
      });

      it('with the eye opposite the surface and the light is offset 45 degrees', () => {
        const eye = new Vector(0, 0, -1);
        const normal = new Vector(0, 0, -1);
        const light = new PointLight(new Point(0, 10, -10), Color.White);
        const result = material.lighting(light, position, eye, normal);
        expect(result.isEqualTo(new Color(0.7364, 0.7364, 0.7364))).toBeTrue();
      });

      it('with the eye in the path of the reflection vector', () => {
        const eye = new Vector(0, -Math.SQRT2 / 2, -Math.SQRT2 / 2);
        const normal = new Vector(0, 0, -1);
        const light = new PointLight(new Point(0, 10, -10), Color.White);
        const result = material.lighting(light, position, eye, normal);
        expect(result.isEqualTo(new Color(1.6364, 1.6364, 1.6364))).toBeTrue();
      });

      it('with the light behind the surface', () => {
        const eye = new Vector(0, 0, -1);
        const normal = new Vector(0, 0, -1);
        const light = new PointLight(new Point(0, 0, 10), Color.White);
        const result = material.lighting(light, position, eye, normal);
        expect(result).toEqual(new Color(0.1, 0.1, 0.1));
      });
    });
  });
});
