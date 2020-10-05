import { Color } from './Color';
import { Point } from './PointVector';

export abstract class Light {
  public readonly position: Point;
  public readonly intensity: Color;

  protected constructor(position: Point, intensity: Color) {
    this.position = position;
    this.intensity = intensity;
  }
}

export class PointLight extends Light {
  constructor(position: Point, intensity: Color) {
    super(position, intensity);
  }
}
