#ifndef CPRACTICE_SPECK_H
#define CPRACTICE_SPECK_H

#include <cstdint>
#include <iomanip>
#include <iostream>

#define ROTL64(x,r) (((x)<<(r)) | (x>>(64-(r))))
#define ROTR64(x,r) (((x)>>(r)) | ((x)<<(64-(r))))

#define ER64(x,y,k) (x=ROTR64(x,8), x+=y, x^=k, y=ROTL64(y,3), y^=x)
#define DR64(x,y,k) (y^=x, y=ROTR64(y,3), x^=k, x-=y, x=ROTL64(x,8))


class SPECK {
public:
    SPECK(uint8_t* key) {
        this->key   = new uint64_t[2];
        rk          = new uint64_t[32];

        this->key[0] = reinterpret_cast<uint64_t*>(key)[0];
        this->key[1] = reinterpret_cast<uint64_t*>(key)[1];

        std::cout << std::hex << std::setfill('0');
        std::cout << "Key 0         : 0x" << std::setw(16) << this->key[0] << '\n';
        std::cout << "Key 1         : 0x" << std::setw(16) << this->key[1] << '\n';

        keySchedule();
    }
    void keySchedule() {
        uint64_t B = key[1], A = key[0];

        for (int i=0; i<31; i++) {
            rk[i] = A;
            ER64(B, A, i);
        }
        rk[31] = A;

    }
    void encrypt(uint8_t* plainText, uint8_t* cipherText) {
        auto Pt = reinterpret_cast<uint64_t*>(plainText);
        auto Ct = reinterpret_cast<uint64_t*>(cipherText);

        Ct[0] = Pt[0]; Ct[1] = Pt[1];
        for (int i=0; i<32; i++)
            ER64(Ct[1], Ct[0], rk[i]);

        cipherText = reinterpret_cast<uint8_t*>(Ct);
    }
    void decrypt(uint8_t* cipherText, uint8_t* plainText) {
        auto Ct = reinterpret_cast<uint64_t*>(cipherText);
        auto Pt = reinterpret_cast<uint64_t*>(plainText);

        Pt[0] = Ct[0]; Pt[1] = Ct[1];
        for (int i=31; i>=0; i--)
            DR64(Pt[1], Pt[0], rk[i]);

        plainText = reinterpret_cast<uint8_t*>(Pt);
    }
    ~SPECK() {
        delete[] key;
        delete[] rk;
    }
private:
    uint64_t* key;
    uint64_t* rk;
};


#endif //CPRACTICE_SPECK_H
