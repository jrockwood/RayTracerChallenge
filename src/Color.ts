import { floatEqual } from './Math';

export class Color {
  public readonly red: number;
  public readonly green: number;
  public readonly blue: number;

  public constructor(red: number, green: number, blue: number) {
    this.red = red;
    this.green = green;
    this.blue = blue;
  }

  public isEqualTo(other: Color): boolean {
    return floatEqual(this.red, other.red) && floatEqual(this.green, other.green) && floatEqual(this.blue, other.blue);
  }

  public add(color: Color): Color {
    return new Color(this.red + color.red, this.green + color.green, this.blue + color.blue);
  }

  public subtract(color: Color): Color {
    return new Color(this.red - color.red, this.green - color.green, this.blue - color.blue);
  }

  public multiply(other: number | Color): Color {
    if (typeof other === 'number') {
      return new Color(this.red * other, this.green * other, this.blue * other);
    }

    return new Color(this.red * other.red, this.green * other.green, this.blue * other.blue);
  }
}
