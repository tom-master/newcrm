(function (f) {
    f.bubbler = function (l, t) {
        var a = this,
        h = f(l),
        u = f(window);
        a.el = l;
        var c = h.width(),
        d = h.height(),
        m,
        n;
        h.data("bubbler", a);
        a.init = function () {
            a.options = f.extend({},
            f.bubbler.defaultOptions, t);
            h.css({
                'background-image': 'url(http://www.17sucai.com/preview/120111/2014-07-10/1402393818279_zcool.com_.cn_/assets/img/3.jpg)',
                overflow: "hidden",
                position: "absolute",
            });
            m = c > d ? Math.floor(d * a.options.max) : Math.floor(c * a.options.max);
            n = c > d ? Math.floor(d * a.options.min) : Math.floor(c * a.options.min);
            for (var e = 0; e < this.options.ammount; e++) {
                var k;
                k = f("<bubble></bubble>");
                var b = Math.floor(Math.random() * m + n),
                g = 2 * Math.random() | 0 ? p(a.options.color, 3 * Math.random() + 2) : p(a.options.color, -1 * (3 * Math.random() + 2)),
                r = q(b),
                g = f.extend({
                    display: "block",
                    position: "absolute",
                    width: b,
                    height: b,
                    background: g,
                    left: r[0],
                    top: r[1],
                    opacity: 0.8
                },
                v(a.options.style, b, g));
                k.css(g);
                k.size = b;
                h.append(k);
                s(k);
            }
            u.bind("resize", w());
        };
        var w = function () {
            c = h.width();
            d = h.height();
            m = c > d ? Math.floor(d * a.options.max) : Math.floor(c * a.options.max);
            n = c > d ? Math.floor(d * a.options.min) : Math.floor(c * a.options.min);
        },
        v = function (e) {
            switch (e) {
                case "circle":
                    return {
                        "border-radius":
                        "50%"
                    };
                default:
                    return {};
            }
        },
        p = function (e, a) {
            var b = parseInt(e.slice(1), 16),
            g = Math.round(2.55 * a),
            c = (b >> 16) + g,
            d = (b >> 8 & 255) + g,
            b = (b & 255) + g;
            return '#' + ('00000' + (Math.random() * 0x1000000 << 0).toString(16)).slice(-6);
        },
        q = function (e) {
            var a = Math.floor(Math.random() * (c + e)) - e;
            e = Math.floor(Math.random() * (d + e)) - e;
            return [a, e];
        },
        s = function (e) {
            var c = q(e.size),
            b = e.offset(),
            d = {},
            f = {
                x: b.left,
                y: b.top
            },
            b = {
                x: b.left,
                y: b.top
            };
            a.options.horizontal && (d.left = c[0], b.x = c[0]);
            a.options.vertical && (d.top = c[1], b.y = c[1]);
            e.animate(d, x(y(f, b)),
                function () {
                    s(e);
                });
        },
        y = function (a, c) {
            var b = 0,
            d = 0,
            b = c.x - a.x,
            d = c.y - a.y;
            return Math.sqrt(b * b + d * d);
        },
        x = function (e) {
            return 1E3 * a.options.time / (c > d ? c : d) * e;
        };
        a.init();
    };
    f.bubbler.defaultOptions = {
        color: "#1D2833",
        ammount: 20,
        min: 0.05,
        max: 0.1,
        time: 100,
        vertical: true,
        horizontal: true,
        style: "circle"
    };
    f.fn.bubbler = function (l) {
        return this.each(function () {
            f.bubbler(this, l);
        });
    };
})(jQuery);