import { add } from '../src/tuples';

describe('add', () => {
  it('should add two numbers', () => {
    expect(add(1, 3)).toBe(4);
  });
});
