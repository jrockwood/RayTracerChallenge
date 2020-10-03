const Epsilon = 0.00001;

export function floatEqual(a: number, b: number): boolean {
  return Math.abs(a - b) < Epsilon;
}

export function clamp(x: number, min: number, max: number): number {
  return Math.min(max, Math.max(x, min));
}
