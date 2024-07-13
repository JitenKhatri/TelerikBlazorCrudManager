export function get(key) {
    // this.CallComponentLoad();
    return window.sessionStorage.getItem(key);
}

export function set(key, value) {
    window.sessionStorage.setItem(key, value);
}

export function clear() {
    window.sessionStorage.clear();
}

export function remove(key) {
    window.sessionStorage.removeItem(key);
}

//export function CallComponentLoad() {
//    DotNet.invokeMethodAsync("BlazorImplementation", "ComponentLoad");
//}

//export function CallLocalComponentMethod(componentInstance) {
//    componentInstance.invokeMethodAsync("ComponentLoad");
//}