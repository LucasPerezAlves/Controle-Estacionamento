(function () {
    var themeToggle = document.getElementById('theme-toggle');

    if (themeToggle) {
        themeToggle.addEventListener('click', function () {
            var isDark = document.documentElement.classList.toggle('dark');
            localStorage.theme = isDark ? 'dark' : 'light';
        });
    }
})();

(function () {
    var elementos = document.querySelectorAll('.tempo-estacionado');

    if (elementos.length === 0) {
        return;
    }

    function formatarDuracao(dataEntrada) {
        var diffMs = Date.now() - dataEntrada.getTime();
        var minutosTotais = Math.max(0, Math.floor(diffMs / 60000));
        var horas = Math.floor(minutosTotais / 60);
        var minutos = minutosTotais % 60;
        return horas > 0 ? horas + 'h ' + minutos + 'min' : minutos + 'min';
    }

    function atualizar() {
        elementos.forEach(function (elemento) {
            var dataEntrada = new Date(elemento.dataset.entrada);
            elemento.textContent = formatarDuracao(dataEntrada);
        });
    }

    atualizar();
    setInterval(atualizar, 30000);
})();
