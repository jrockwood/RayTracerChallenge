import { timeStamp } from 'console';

export class Rect {
  public readonly top: number;
  public readonly left: number;
  public readonly bottom: number;
  public readonly right: number;

  public constructor(top: number, left: number, bottom: number, right: number) {
    if (right < left) {
      throw new Error(`Left must be less than right: left=${left}, right=${right}`);
    }

    if (bottom < top) {
      throw new Error(`Top must be less than bottom: top=${top}, bottom=${bottom}`);
    }

    this.top = top;
    this.left = left;
    this.bottom = bottom;
    this.right = right;
  }

  public width(): number {
    return this.right - this.left;
  }

  public height(): number {
    return this.bottom - this.top;
  }
}
