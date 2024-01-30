function balikDanSplit(input) {
    let length = input.length;
    let middle = Math.floor(length / 2);
    let array = input.split('');

    // Balik kiri
    for (let i = 0; i < middle / 2; i++) {
        let temp = array[i];
        array[i] = array[middle - i - 1];
        array[middle - i - 1] = temp;
    }

    // Balik kanan
    for (let i = middle; i < (middle + length) / 2; i++) {
        let temp = array[i];
        array[i] = array[length - i + middle - 1];
        array[length - i + middle - 1] = temp;
    }

    return array.join('');
}

function prosesData() {
    let inputData = document.getElementById('inputData').value;

    // Pastikan panjang input adalah 6 karakter
    if (inputData.length === 6) {
        let outputResult = balikDanSplit(inputData);
        document.getElementById('outputResult').textContent = `Hasil: ${outputResult}`;
    } else {
        document.getElementById('outputResult').textContent = 'Masukkan harus terdiri dari 6 karakter.';
    }
}
