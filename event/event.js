class EventEmit {

    constructor() {
        this.handlers = {}
    }

    on(name, fn) {
        if (!this.handlers[name]) {
            this.handlers[name] = []
        }
        this.handlers[name].push(fn)
    }

    emit(name, data) {
        this.handlers[name]?.forEach(fn => fn(data))
    }

    off(name, fn) {
        let handlers = this.handlers[name]
        if (!handlers) return
        if (!fn) {
            handlers && (handlers.length = 0)
        } else {
            const index = handlers.findIndex(item => item === fn)
            index && handlers.splice(index, 1)
        }
    }

}