import { Color } from './Color';
import { Light } from './Lights';
import { Point, Vector } from './PointVector';

export class Material {
  public static readonly defaultColor = Color.White;
  public static readonly defaultAmbient = 0.1;
  public static readonly defaultDiffuse = 0.9;
  public static readonly defaultSpecular = 0.9;
  public static readonly defaultShininess = 200.0;

  public readonly color: Color;
  public readonly ambient: number;
  public readonly diffuse: number;
  public readonly specular: number;
  public readonly shininess: number;

  public constructor(
    color = Material.defaultColor,
    ambient = Material.defaultAmbient,
    diffuse = Material.defaultDiffuse,
    specular = Material.defaultSpecular,
    shininess = Material.defaultShininess,
  ) {
    this.color = color;
    this.ambient = Material.verifyValue(ambient, 'ambient');
    this.diffuse = Material.verifyValue(diffuse, 'diffuse');
    this.specular = Material.verifyValue(specular, 'specular');
    this.shininess = Material.verifyValue(shininess, 'shininess');
  }

  private static verifyValue(value: number, valueName: string): number {
    if (value < 0) {
      throw new Error(`${valueName} must be non-negative: ${value}`);
    }
    return value;
  }

  public withColor(value: Color): Material {
    return new Material(value, this.ambient, this.diffuse, this.specular, this.shininess);
  }

  public withAmbient(value: number): Material {
    return new Material(this.color, value, this.diffuse, this.specular, this.shininess);
  }

  public withDiffuse(value: number): Material {
    return new Material(this.color, this.ambient, value, this.specular, this.shininess);
  }

  public withSpecular(value: number): Material {
    return new Material(this.color, this.ambient, this.diffuse, value, this.shininess);
  }

  public withShininess(value: number): Material {
    return new Material(this.color, this.ambient, this.diffuse, this.specular, value);
  }

  public lighting(light: Light, position: Point, eye: Vector, normal: Vector, isInShadow: boolean): Color {
    // Combine the surface color with the light's color/intensity.
    const effectiveColor = this.color.multiply(light.intensity);

    // Find the direction to the light source.
    const lightVector = light.position.subtract(position).normalize();

    // Compute the ambient contribution.
    const ambient = effectiveColor.multiply(this.ambient);

    // lightDotNormal represents the cosine of the angle between the light vector and the normal
    // vector. A negative number means the light is on the other side of the surface.
    const lightDotNormal = lightVector.dot(normal);

    let diffuse: Color;
    let specular: Color;

    if (lightDotNormal < 0) {
      diffuse = Color.Black;
      specular = Color.Black;
    } else {
      // Compute the diffuse contribution.
      diffuse = effectiveColor.multiply(this.diffuse).multiply(lightDotNormal);

      // reflectDotEye represents the cosine of the angle between the reflection vector and the eye
      // vector. A negative number means the light reflects away from the eye.
      const reflectVector = lightVector.negate().reflect(normal);
      const reflectDotEye = reflectVector.dot(eye);

      if (reflectDotEye <= 0) {
        specular = Color.Black;
      } else {
        // Compute the specular contribution.
        const factor = Math.pow(reflectDotEye, this.shininess);
        specular = light.intensity.multiply(this.specular * factor);
      }
    }

    // Add the three contributions together to get the final shading.
    const result = isInShadow ? ambient : ambient.add(diffuse).add(specular);
    return result;
  }
}
