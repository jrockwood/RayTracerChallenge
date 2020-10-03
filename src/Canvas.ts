import * as fs from 'fs-extra';
import * as path from 'path';
import { clamp } from './Math';
import { Color } from './Color';
import { Rect } from './Rect';

export class Canvas {
  public readonly width: number;
  public readonly height: number;

  private _pixels: Color[][];

  public constructor(width: number, height: number) {
    this.width = width;
    this.height = height;

    this._pixels = [];
    for (let y = 0; y < height; y++) {
      this._pixels[y] = [];
    }

    this.clear();
  }

  public getPixel(x: number, y: number): Color {
    this.verifyIndex(x, y);
    return this._pixels[y][x];
  }

  public clear(color: Color = Color.Black): void {
    for (let y = 0; y < this.height; y++) {
      for (let x = 0; x < this.width; x++) {
        this._pixels[y][x] = color;
      }
    }
  }

  public setPixel(x: number, y: number, color: Color): void {
    this.verifyIndex(x, y);
    this._pixels[y][x] = color;
  }

  public fillRect(rect: Rect, color: Color): void {
    for (let x = Math.max(0, rect.left); x <= Math.min(rect.right, this.width - 1); x++) {
      for (let y = Math.max(0, rect.top); y <= Math.min(rect.bottom, this.height - 1); y++) {
        this.setPixel(x, y, color);
      }
    }
  }

  public toPpm(): string {
    const builder = new PortablePixmapBuilder(this.width, this.height);

    for (let y = 0; y < this.height; y++) {
      for (let x = 0; x < this.width; x++) {
        const color = this.getPixel(x, y);
        builder.appendColor(color);
      }
    }

    return builder.toPpmFormat();
  }

  public saveToPpmFile(fileName: string): void {
    const contents = this.toPpm();
    fs.ensureDirSync(path.dirname(fileName));
    fs.writeFileSync(fileName, contents);
  }

  private verifyIndex(x: number, y: number): void {
    if (x < 0 || x >= this.width || y < 0 || y >= this.height) {
      throw new Error(`Index out of bounds: x=${x}, y=${y}`);
    }
  }
}

class PortablePixmapBuilder {
  public readonly maxLineLength: number = 70;
  public readonly width: number;
  public readonly height: number;

  private _lines: string[] = [''];

  public constructor(width: number, height: number) {
    this.width = width;
    this.height = height;
  }

  public appendColor(color: Color): PortablePixmapBuilder {
    const scaledColor = color.multiply(255);
    return this.appendColorComponent(scaledColor.red)
      .appendColorComponent(scaledColor.green)
      .appendColorComponent(scaledColor.blue);
  }

  public toPpmFormat(): string {
    const header = `P3\n${this.width} ${this.height}\n255\n`;
    return header + this._lines.join('\n') + '\n';
  }

  private appendColorComponent(scaledComponent: number): PortablePixmapBuilder {
    const clamped = clamp(Math.round(scaledComponent), 0, 255).toString();

    const currentLine = this._lines[this._lines.length - 1];
    const proposedLine = `${currentLine}${currentLine.length > 0 ? ' ' : ''}${clamped}`;
    if (proposedLine.length > this.maxLineLength) {
      this._lines.push(clamped);
    } else {
      this._lines[this._lines.length - 1] = proposedLine;
    }

    return this;
  }
}
