import { ValueEditor } from '@easydata/core'
import { Expression } from '@easyquery/core'
import { ExpressionRenderer, QueryPanel } from '@easyquery/ui'

export class CustomExpressionRenderer extends ExpressionRenderer {

  constructor(queryPanel: QueryPanel, expression: Expression, valueEditor: ValueEditor, slot?: HTMLDivElement) {
      super(queryPanel, expression, valueEditor, slot);
  }

  protected renderEditor(): void {
    // Build your editor here
  }

  protected showEditor(): void {
    // show your editor
    const value = prompt("Enter value: ");
    if (value) {
      this.setValue(value);
    }
  }

  protected closeEditor(): void {
    // hide your editor
  }

}
