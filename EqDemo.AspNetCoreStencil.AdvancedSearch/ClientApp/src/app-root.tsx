import { Component, Element, h } from '@stencil/core';
import '@stencil/router';

@Component({
    tag: 'app-root'
})
export class AppRoot {

    @Element() host: HTMLDivElement;

    componentWillLoad() {
        this.host.innerHTML = '';
    }

    render() {
        return (
            <app-layout>
                <stencil-router>
                    <stencil-route-switch scrollTopOffset={0}>
                        <stencil-route url="/" component="app-home" exact={true} />
                        <stencil-route url="/counter" component="app-counter" exact={true} />
                        <stencil-route url="/fetch-data" component="fetch-data" exact={true}/>
                        <stencil-route url="/easy-query" component="easy-query" exact={true}/>
                        <stencil-route url="/filter-bar" component="filter-bar" exact={true}/>
                    </stencil-route-switch>
                </stencil-router>
            </app-layout>
        );
    }
}