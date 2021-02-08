import { Component, h } from '@stencil/core';

@Component({
    tag: 'app-layout',
})
export class Layout {

    render() {
        return (
            <div>
                <nav-menu />
                <div class="container">
                    <slot />
                </div>
            </div>
        );
    }
}