/* eslint-disable */
/* tslint:disable */
/**
 * This is an autogenerated file created by the Stencil compiler.
 * It contains typing information for all components that exist in this project.
 */
import { HTMLStencilElement, JSXBase } from "@stencil/core/internal";
export namespace Components {
    interface AppCounter {
    }
    interface AppHome {
    }
    interface AppLayout {
    }
    interface AppRoot {
    }
    interface EasyQuery {
    }
    interface FetchData {
    }
    interface FilterBar {
    }
    interface NavMenu {
    }
}
declare global {
    interface HTMLAppCounterElement extends Components.AppCounter, HTMLStencilElement {
    }
    var HTMLAppCounterElement: {
        prototype: HTMLAppCounterElement;
        new (): HTMLAppCounterElement;
    };
    interface HTMLAppHomeElement extends Components.AppHome, HTMLStencilElement {
    }
    var HTMLAppHomeElement: {
        prototype: HTMLAppHomeElement;
        new (): HTMLAppHomeElement;
    };
    interface HTMLAppLayoutElement extends Components.AppLayout, HTMLStencilElement {
    }
    var HTMLAppLayoutElement: {
        prototype: HTMLAppLayoutElement;
        new (): HTMLAppLayoutElement;
    };
    interface HTMLAppRootElement extends Components.AppRoot, HTMLStencilElement {
    }
    var HTMLAppRootElement: {
        prototype: HTMLAppRootElement;
        new (): HTMLAppRootElement;
    };
    interface HTMLEasyQueryElement extends Components.EasyQuery, HTMLStencilElement {
    }
    var HTMLEasyQueryElement: {
        prototype: HTMLEasyQueryElement;
        new (): HTMLEasyQueryElement;
    };
    interface HTMLFetchDataElement extends Components.FetchData, HTMLStencilElement {
    }
    var HTMLFetchDataElement: {
        prototype: HTMLFetchDataElement;
        new (): HTMLFetchDataElement;
    };
    interface HTMLFilterBarElement extends Components.FilterBar, HTMLStencilElement {
    }
    var HTMLFilterBarElement: {
        prototype: HTMLFilterBarElement;
        new (): HTMLFilterBarElement;
    };
    interface HTMLNavMenuElement extends Components.NavMenu, HTMLStencilElement {
    }
    var HTMLNavMenuElement: {
        prototype: HTMLNavMenuElement;
        new (): HTMLNavMenuElement;
    };
    interface HTMLElementTagNameMap {
        "app-counter": HTMLAppCounterElement;
        "app-home": HTMLAppHomeElement;
        "app-layout": HTMLAppLayoutElement;
        "app-root": HTMLAppRootElement;
        "easy-query": HTMLEasyQueryElement;
        "fetch-data": HTMLFetchDataElement;
        "filter-bar": HTMLFilterBarElement;
        "nav-menu": HTMLNavMenuElement;
    }
}
declare namespace LocalJSX {
    interface AppCounter {
    }
    interface AppHome {
    }
    interface AppLayout {
    }
    interface AppRoot {
    }
    interface EasyQuery {
    }
    interface FetchData {
    }
    interface FilterBar {
    }
    interface NavMenu {
    }
    interface IntrinsicElements {
        "app-counter": AppCounter;
        "app-home": AppHome;
        "app-layout": AppLayout;
        "app-root": AppRoot;
        "easy-query": EasyQuery;
        "fetch-data": FetchData;
        "filter-bar": FilterBar;
        "nav-menu": NavMenu;
    }
}
export { LocalJSX as JSX };
declare module "@stencil/core" {
    export namespace JSX {
        interface IntrinsicElements {
            "app-counter": LocalJSX.AppCounter & JSXBase.HTMLAttributes<HTMLAppCounterElement>;
            "app-home": LocalJSX.AppHome & JSXBase.HTMLAttributes<HTMLAppHomeElement>;
            "app-layout": LocalJSX.AppLayout & JSXBase.HTMLAttributes<HTMLAppLayoutElement>;
            "app-root": LocalJSX.AppRoot & JSXBase.HTMLAttributes<HTMLAppRootElement>;
            "easy-query": LocalJSX.EasyQuery & JSXBase.HTMLAttributes<HTMLEasyQueryElement>;
            "fetch-data": LocalJSX.FetchData & JSXBase.HTMLAttributes<HTMLFetchDataElement>;
            "filter-bar": LocalJSX.FilterBar & JSXBase.HTMLAttributes<HTMLFilterBarElement>;
            "nav-menu": LocalJSX.NavMenu & JSXBase.HTMLAttributes<HTMLNavMenuElement>;
        }
    }
}