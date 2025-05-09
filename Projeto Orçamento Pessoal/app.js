class Despesa {
    constructor(ano, mes, dia, tipo, descricao, valor) {
        this.ano = ano
        this.mes = mes
        this.dia = dia
        this.tipo = tipo
        this.descricao = descricao
        this.valor = valor
    }

    validarDados() {
        for (let i in this) {
            if (this[i] == undefined || this[i] == '' || this[i] == null) {
                return false
            }
        }
        return true
    }
}

class Bd {
    constructor() {
        let id = localStorage.getItem('id');
        id = Number(id);
        if (isNaN(id) || id < 0) {
            id = 0;
            localStorage.setItem('id', id);
        }
    }

    getProximoId() {
        let id = Number(localStorage.getItem('id'));
        if (isNaN(id) || id < 0) id = 0;
        return id + 1;
    }

    gravar(d) {
        let id = this.getProximoId();
        localStorage.setItem(id, JSON.stringify(d));  // Armazena a despesa com o ID gerado
        localStorage.setItem('id', id); // Atualiza o ID no localStorage
    }

    recuperarTodosRegistros() {
        let despesas = [];
        let id = Number(localStorage.getItem('id'));
        if (isNaN(id) || id < 0) id = 0;

        for (let i = 1; i <= id; i++) {
            let despesa = localStorage.getItem(i);
            if (despesa !== null) {
                try {
                    despesas.push(JSON.parse(despesa));
                } catch (e) {
                    // Se o dado estiver corrompido, apenas ignore
                }
            }
        }
        return despesas;
    }
}

let bd = new Bd();

function cadastrarDespesa() {
    let ano = document.getElementById('ano');
    let mes = document.getElementById('mes');
    let dia = document.getElementById('dia');
    let tipo = document.getElementById('tipo');
    let descricao = document.getElementById('descricao');
    let valor = document.getElementById('valor');

    let despesa = new Despesa(
        ano.value,
        mes.value,
        dia.value,
        tipo.value,
        descricao.value,
        valor.value
    );

    if (despesa.validarDados()) {
        bd.gravar(despesa);

        document.getElementById('modal_titulo').innerHTML = 'Registro inserido com sucesso';
        document.getElementById('modal_titulo_div').className = 'modal-header text-success';
        document.getElementById('modal_conteudo').innerHTML = 'Despesa registrada com sucesso';
        document.getElementById('modal_btn').innerHTML = 'Voltar';
        document.getElementById('modal_btn').className = 'btn btn-success';
        //dialog de sucesso
        $('#modalRegistraDespesa').modal('show');
    } else {
        //dialog de erro
        document.getElementById('modal_titulo').innerHTML = 'Erro na inclusão do registro';
        document.getElementById('modal_titulo_div').className = 'modal-header text-danger';
        document.getElementById('modal_conteudo').innerHTML = 'Preencha todos os campos';
        document.getElementById('modal_btn').innerHTML = 'Voltar e corrigir';
        document.getElementById('modal_btn').className = 'btn btn-danger';
        $('#modalRegistraDespesa').modal('show');
    }
}

function carregarListaDespesas() {
    let despesas = Array();

    despesas = bd.recuperarTodosRegistros();

    let listaDespesas = document.getElementById('listaDespesas');

    // percorrer o array despesas, listando cada despesa de forma dinâmica
    despesas.forEach(function (d) {
        let linha = listaDespesas.insertRow();

        linha.insertCell(0).innerHTML = `${d.dia}/${d.mes}/${d.ano}`;
        switch (d.tipo) {
            case '1': d.tipo = 'Alimentação';
                break;
            case '2': d.tipo = 'Educação';
                break;
            case '3': d.tipo = 'Lazer';
                break;
            case '4': d.tipo = 'Saúde';
                break;
            case '5': d.tipo = 'Transporte';
                break;
        }
        linha.insertCell(1).innerHTML = d.tipo;
        linha.insertCell(2).innerHTML = d.descricao;
        linha.insertCell(3).innerHTML = d.valor;
    });
}