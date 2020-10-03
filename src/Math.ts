const Epsilon = 0.00001;

export function floatEqual(a: number, b: number): boolean {
  return Math.abs(a - b) < Epsilon;
}
