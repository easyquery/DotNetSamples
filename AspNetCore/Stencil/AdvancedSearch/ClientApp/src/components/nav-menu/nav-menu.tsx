import { Component, State, h } from '@stencil/core';

@Component({
    tag: 'nav-menu',
    styleUrl: 'nav-menu.css'
})
export class NavMenu {

    @State() isExpanded = false;

    toggle = () => {
        this.isExpanded = !this.isExpanded;
    }

    render() {

        let collapseClass = "navbar-collapse collapse d-sm-inline-flex flex-sm-row-reverse";
        if (this.isExpanded) {
            collapseClass += " show";
        }

        return (
            <header>
                <nav class='navbar navbar-expand-sm navbar-toggleable-sm navbar-light bg-white border-bottom box-shadow mb-3'>
                    <div class="container">
                        <stencil-route-link anchorClass="navbar-brand" url="/">
                            EqStencilDemo
                        </stencil-route-link>
                        <button class="navbar-toggler" type="button" aria-expanded={this.isExpanded} data-toggle="collapse" data-target=".navbar-collapse" aria-label="Toggle navigation" onClick={this.toggle}>
                            <span class="navbar-toggler-icon"></span>
                        </button>
                        <div class={collapseClass}>
                            <ul class="navbar-nav flex-grow">
                                <li class="nav-item">
                                    <stencil-route-link anchorClass="nav-link text-dark" activeClass="link-active" url="/">
                                        Home
                                    </stencil-route-link>
                                </li>
                                <li class="nav-item">
                                    <stencil-route-link anchorClass="nav-link text-dark" activeClass="link-active" url="/counter">
                                        Counter
                                    </stencil-route-link>
                                </li>
                                <li class="nav-item">
                                    <stencil-route-link anchorClass="nav-link text-dark" activeClass="link-active" url="/fetch-data">
                                        Fetch Data
                                    </stencil-route-link>
                                </li>
                                <li class="nav-item">
                                    <stencil-route-link anchorClass="nav-link text-dark" activeClass="link-active" url="/easy-query">
                                        EasyQuery
                                    </stencil-route-link>
                                </li>
                                <li class="nav-item">
                                    <stencil-route-link anchorClass="nav-link text-dark" activeClass="link-active" url="/filter-bar">
                                        Data Filter
                                    </stencil-route-link>
                                </li>
                            </ul>
                        </div>
                    </div>
                </nav>
            </header>
        )
    }
}