import { Component, State, h } from "@stencil/core";

@Component({
    tag: 'app-counter'
})
export class Counter {
    static displayName = Counter.name;

    @State() currentCount: number;
  
    constructor () {
      this.currentCount = 0;
      this.incrementCounter = this.incrementCounter.bind(this);
    }
  
    incrementCounter () {
      this.currentCount++;
    }
  
    render () {
      return (
        <div>
          <h1>Counter</h1>
  
          <p>This is a simple example of a Stencil component.</p>
  
          <p>Current count: <strong>{this.currentCount}</strong></p>
  
          <button class="btn btn-primary" onClick={this.incrementCounter}>Increment</button>
        </div>
      );
    }
  }
  