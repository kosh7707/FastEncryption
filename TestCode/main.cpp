#include <iomanip>
#include "AES.h"
#include "ARIA.h"
#include "HIGHT.h"
#include "TWINE.h"

void printHex(uint8_t* data, int len) {
    for (int i = 0; i < len; ++i) {
        std::cout << std::hex << std::setw(2) << std::setfill('0') << std::uppercase << (int)data[i] << " ";
    }
    std::cout << std::endl;
}

void test_aes() {
    uint8_t key[16] = {0x2b, 0x7e, 0x15, 0x16, 0x28, 0xae, 0xd2, 0xa6, 0xab, 0xf7, 0x15, 0x88, 0x09, 0xcf, 0x4f, 0x3c};
    AES aes(key);

    uint8_t plainText[16] = {0x65, 0x66, 0x67, 0x68, 0x69, 0x70, 0x71, 0x72, 0x73, 0x74, 0x75, 0x76, 0x77, 0x78, 0x79, 0x80};

    // encrypt
    auto encryptText = aes.Encrypt(plainText);
    std::cout << "cipherText: \t";
    printHex(encryptText, 16);

    // decrypt
    auto decryptText = aes.Decrypt(encryptText);
    std::cout << "plaintext: \t";
    printHex(decryptText, 16);
}

void test_aria() {
    ARIA aria;
    aria.test();
}

void test_hight() {
    HIGHT hight;

    uint8_t pdwRoundKey[136] = {0,};																					// Round keys for encryption or decryption
    uint8_t pbUserKey[16] = {0x88, 0xE3, 0x4F, 0x8F, 0x08, 0x17, 0x79, 0xF1,
                             0xE9, 0xF3, 0x94, 0x37, 0x0A, 0xD4, 0x05, 0x89 }; 		// User secret key (128bits)
    uint8_t pbData[8]     = {0xD7, 0x6D, 0x0D,0x18, 0x32, 0x7E, 0xC5, 0x62}; 			// input plaintext to be encrypted (64bits)

    std::cout << "Key           : ";
    printHex(pbUserKey, 16);

    std::cout << "PlainText     : ";
    printHex(pbData, 8);

    hight.HIGHT_KeySched(pbUserKey, pdwRoundKey);

    std::cout << "\n\nEncryption...\n";
    hight.HIGHT_Encrypt(pdwRoundKey, pbData);

    std::cout << "CipherText    : ";
    printHex(pbData, 8);

    std::cout << "\n\nDecryption...\n";
    hight.HIGHT_Decrypt(pdwRoundKey, pbData);

    std::cout << "PlainText     : ";
    printHex(pbData, 8);

}

void test_twine() {
    uint8_t key[16] = {0x00, 0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77, 0x88, 0x99, 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF};
    uint8_t plainText[8] = {0x01, 0x23, 0x45, 0x67, 0x89, 0xab, 0xcd, 0xef};
    uint8_t cipherText[8] = {0x97, 0x9F, 0xF9, 0xB3, 0x79, 0xB5, 0xA9, 0xB8};
    uint8_t temp[8] = {};

    TWINE twine(key);

    std::cout << "Key           : ";
    printHex(key, 16);

    std::cout << "PlainText     : ";
    printHex(plainText, 8);

    std::cout << "\n\nEncryption...\n";
    twine.encrypt(plainText, temp);

    std::cout << "CipherText    : ";
    printHex(temp, 8);
    std::cout << "Expected      : ";
    printHex(cipherText, 8);

    std::cout << "\n\nDecryption...\n";
    twine.decrypt(cipherText, temp);

    std::cout << "PlainText     : ";
    printHex(temp, 8);
    std::cout << "Expected      : ";
    printHex(plainText, 8);
}

void test_speck() {

}

int main() {
//    test_aes();
//    test_aria();
//    test_hight();
//    test_twine();
    test_speck();
}