#include "AES.h"

AES::AES(uint8_t* key) : key(key) {
    state       = new uint8_t[16];
    roundKey    = new uint8_t[176];
    KeyExpansion();
}

AES::~AES() {
    delete[] state;
    delete[] roundKey;
}

void AES::KeyExpansion() {
    for (int i=0; i<4; i++) {
        roundKey[i * 4 + 0] = key[i * 4 + 0];
        roundKey[i * 4 + 1] = key[i * 4 + 1];
        roundKey[i * 4 + 2] = key[i * 4 + 2];
        roundKey[i * 4 + 3] = key[i * 4 + 3];
    }

    uint8_t temp[4], k;
    for (int i=4; i<44; i++) {
        temp[0] = roundKey[(i-1) * 4 + 0];
        temp[1] = roundKey[(i-1) * 4 + 1];
        temp[2] = roundKey[(i-1) * 4 + 2];
        temp[3] = roundKey[(i-1) * 4 + 3];

        if (i % 4 == 0) {
            // RotWord
            k = temp[0];
            temp[0] = temp[1];
            temp[1] = temp[2];
            temp[2] = temp[3];
            temp[3] = k;
            // SubWord
            temp[0] = sbox[temp[0]];
            temp[1] = sbox[temp[1]];
            temp[2] = sbox[temp[2]];
            temp[3] = sbox[temp[3]];
            // rcon xor
            temp[0] = temp[0] ^ rcon[i/4];
        }
        roundKey[i * 4 + 0] = roundKey[(i-4) * 4 + 0] ^ temp[0];
        roundKey[i * 4 + 1] = roundKey[(i-4) * 4 + 1] ^ temp[1];
        roundKey[i * 4 + 2] = roundKey[(i-4) * 4 + 2] ^ temp[2];
        roundKey[i * 4 + 3] = roundKey[(i-4) * 4 + 3] ^ temp[3];
    }
}

void AES::AddRoundKey(uint8_t round) {
    for (int i=0; i<4; i++) {
        for (int j=0; j<4; j++)
            state[i * 4 + j] ^= roundKey[round * 16 + i * 4 + j];
    }
}

uint8_t AES::xtime(uint8_t x) {
    return (x << 1) ^ (((x >> 7) & 1) * 0x1b);
}

void AES::SubBytes() {
    for (int i=0; i<4; i++) {
        for (int j=0; j<4; j++)
            state[i * 4 + j] = sbox[state[i * 4 + j]];
    }
}

void AES::ShiftRows() {
    uint8_t temp;

    // first row
    temp             = state[1 * 4 + 0];
    state[1 * 4 + 0] = state[1 * 4 + 1];
    state[1 * 4 + 1] = state[1 * 4 + 2];
    state[1 * 4 + 2] = state[1 * 4 + 3];
    state[1 * 4 + 3] = temp;

    // second row
    temp             = state[2 * 4 + 0];
    state[2 * 4 + 0] = state[2 * 4 + 2];
    state[2 * 4 + 2] = temp;
    temp             = state[2 * 4 + 1];
    state[2 * 4 + 1] = state[2 * 4 + 3];
    state[2 * 4 + 3] = temp;

    // third row
    temp             = state[3 * 4 + 3];
    state[3 * 4 + 3] = state[3 * 4 + 2];
    state[3 * 4 + 2] = state[3 * 4 + 1];
    state[3 * 4 + 1] = state[3 * 4 + 0];
    state[3 * 4 + 0] = temp;
}

void AES::MixColumns() {
    uint8_t Tmp, Tm, t;
    for (int i=0; i<4; i++) {
        t   = state[0 * 4 + i];
        Tmp = state[0 * 4 + i] ^ state[1 * 4 + i] ^ state[2 * 4 + i] ^ state[3 * 4 + i];
        Tm  = state[0 * 4 + i] ^ state[1 * 4 + i];  Tm = xtime(Tm);    state[0 * 4 + i] ^= Tm ^ Tmp;
        Tm  = state[1 * 4 + i] ^ state[2 * 4 + i];  Tm = xtime(Tm);    state[1 * 4 + i] ^= Tm ^ Tmp;
        Tm  = state[2 * 4 + i] ^ state[3 * 4 + i];  Tm = xtime(Tm);    state[2 * 4 + i] ^= Tm ^ Tmp;
        Tm  = state[3 * 4 + i] ^ t;                 Tm = xtime(Tm);    state[3 * 4 + i] ^= Tm ^ Tmp;
    }
}

void AES::Encrypt() {
    AddRoundKey(0);

    for (uint8_t round = 1; round < 10; round++) {
        SubBytes();
        ShiftRows();
        MixColumns();
        AddRoundKey(round);
    }

    SubBytes();
    ShiftRows();
    AddRoundKey(10);
}

uint8_t* AES::Encrypt(uint8_t* plainText) {
    memcpy(state, plainText, 16);
    Encrypt();
    uint8_t* ret = new uint8_t[16];
    memcpy(ret, state, 16);
    return ret;
}

uint8_t AES::multiply(uint8_t x, uint8_t y) {
    return (((y & 1) * x) ^
            ((y >> 1 & 1) * xtime(x)) ^
            ((y >> 2 & 1) * xtime(xtime(x))) ^
            ((y >> 3 & 1) * xtime(xtime(xtime(x)))) ^
            ((y >> 4 & 1) * xtime(xtime(xtime(xtime(x))))));
}

void AES::InvSubBytes() {
    for (int i=0; i<4; i++) {
        for (int j=0; j<4; j++)
            state[i * 4 + j] = rsbox[state[i * 4 + j]];
    }
}

void AES::InvShiftRows() {
    uint8_t temp;

    // first row
    temp             = state[1 * 4 + 3];
    state[1 * 4 + 3] = state[1 * 4 + 2];
    state[1 * 4 + 2] = state[1 * 4 + 1];
    state[1 * 4 + 1] = state[1 * 4 + 0];
    state[1 * 4 + 0] = temp;

    // second row
    temp             = state[2 * 4 + 0];
    state[2 * 4 + 0] = state[2 * 4 + 2];
    state[2 * 4 + 2] = temp;
    temp             = state[2 * 4 + 1];
    state[2 * 4 + 1] = state[2 * 4 + 3];
    state[2 * 4 + 3] = temp;

    // third row
    temp             = state[3 * 4 + 0];
    state[3 * 4 + 0] = state[3 * 4 + 1];
    state[3 * 4 + 1] = state[3 * 4 + 2];
    state[3 * 4 + 2] = state[3 * 4 + 3];
    state[3 * 4 + 3] = temp;
}

void AES::InvMixColumns() {
    uint8_t t1, t2, t3, t4;
    for (int i=0; i<4; i++) {
        t1 = state[0 * 4 + i]; t2 = state[1 * 4 + i];
        t3 = state[2 * 4 + i]; t4 = state[3 * 4 + i];

        state[0 * 4 + i] = multiply(t1, 0x0e) ^ multiply(t2, 0x0b) ^ multiply(t3, 0x0d) ^ multiply(t4, 0x09);
        state[1 * 4 + i] = multiply(t1, 0x09) ^ multiply(t2, 0x0e) ^ multiply(t3, 0x0b) ^ multiply(t4, 0x0d);
        state[2 * 4 + i] = multiply(t1, 0x0d) ^ multiply(t2, 0x09) ^ multiply(t3, 0x0e) ^ multiply(t4, 0x0b);
        state[3 * 4 + i] = multiply(t1, 0x0b) ^ multiply(t2, 0x0d) ^ multiply(t3, 0x09) ^ multiply(t4, 0x0e);
    }
}

void AES::Decrypt() {
    AddRoundKey(10);

    for (uint8_t round = 9; round > 0; round--) {
        InvShiftRows();
        InvSubBytes();
        AddRoundKey(round);
        InvMixColumns();
    }

    InvShiftRows();
    InvSubBytes();
    AddRoundKey(0);
}

uint8_t* AES::Decrypt(uint8_t* cipherText) {
    memcpy(state, cipherText, 16);
    Decrypt();
    uint8_t* ret = new uint8_t[16];
    memcpy(ret, state, 16);
    return ret;
}
