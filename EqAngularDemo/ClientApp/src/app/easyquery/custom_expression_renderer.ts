import { Expression, ValueEditor } from "@easyquery/core"
import { ExpressionRenderer, QueryPanel } from "@easyquery/ui"

export class CustomExpressionRenderer extends ExpressionRenderer {

  constructor(queryPanel: QueryPanel, expression: Expression, valueEditor: ValueEditor, slot?: HTMLDivElement) {
      super(queryPanel, expression, valueEditor, slot);
  }

  protected renderEditor(): void {
    // Build your editor here
  }

  protected appear(): void {
    // show your editor
    const value = prompt("Enter value: ");
    this.setValue(value);
  }

  protected disappear(): void {
    // hide your editor
  }

}
