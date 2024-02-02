import numpy
import sys

def create_matrix_from(key):
    m = [[0] * 3 for i in range(3)]
    for i in range(3):
        for j in range(3):
            m[i][j] = ord(key[3 * i + j]) % 65
    return m

# C = P*K mod 26
def encrypt(P, K):
    C = [0, 0, 0]
    C[0] = (K[0][0] * P[0] + K[1][0] * P[1] + K[2][0] * P[2]) % 26
    C[1] = (K[0][1] * P[0] + K[1][1] * P[1] + K[2][1] * P[2]) % 26
    C[2] = (K[0][2] * P[0] + K[1][2] * P[1] + K[2][2] * P[2]) % 26
    return C

def Hill(message, K):
    cipher_text = []
    # Transform the message 3 characters at a time
    for i in range(0, len(message), 3):
        P = [0, 0, 0]
        # Assign the corresponding integer value to each letter
        for j in range(3):
            if i + j < len(message):
                P[j] = ord(message[i + j]) % 65
        # Encrypt three letters
        C = encrypt(P, K)
        # Add the encrypted 3 letters to the final cipher text
        for j in range(3):
            cipher_text.append(chr(C[j] + 65))
        # Repeat until all sets of three letters are processed.

    # return a string
    return "".join(cipher_text)

def MatrixInverse(K):
    det = int(numpy.linalg.det(K))
    det_multiplicative_inverse = pow(det, -1, 26)
    K_inv = [[0] * 3 for i in range(3)]
    for i in range(3):
        for j in range(3):
            Dji = K
            Dji = numpy.delete(Dji, (j), axis=0)
            Dji = numpy.delete(Dji, (i), axis=1)
            det = Dji[0][0] * Dji[1][1] - Dji[0][1] * Dji[1][0]
            K_inv[i][j] = (det_multiplicative_inverse * pow(-1, i + j) * det) % 26
    return K_inv

def encrypt_file(input_filename, output_filename, key):
    try:
        with open(input_filename, 'r') as file:
            message = file.read().replace('\n', '')
    except FileNotFoundError:
        print("Input file not found.")
        return

    K = create_matrix_from(key)
    cipher_text = Hill(message, K)

    with open(output_filename, 'w') as file:
        file.write(cipher_text)

def decrypt_file(input_filename, output_filename, key):

    try:
        with open(input_filename, 'r') as file:
            cipher_text = file.read().replace('\n', '')
    except FileNotFoundError:
        print("Input file not found.")
        return

    K = create_matrix_from(key)
    K_inv = MatrixInverse(K)
    plain_text = Hill(cipher_text, K_inv)

    with open(output_filename, 'w') as file:
        file.write(plain_text)

if __name__ == "__main__":
    while True:
        choice = input("Choose an option: 'e' - encrypt, 'd' - decrypt, 'q' - quit: ")


        if choice.upper() == 'E':
            input_file = input("Input the input file name: ")
            output_file = input("Input the output file name: ")
            key = input("Input the key: ").upper()
            encrypt_file(input_file, output_file, key)
            print(f"\nDigital key: {create_matrix_from(key)}")
            print(f"✅ {output_file} created, encryption completed successfully!")
            break
        elif choice.upper() == 'D':
            input_file = input("Input the input file name: ")
            output_file = input("Input the output file name: ")
            key = input("Input the key: ").upper()
            decrypt_file(input_file, output_file, key)
            print(f"\nDigital key: {create_matrix_from(key)}")
            print(f"\n✅ {output_file} created, encryption completed successfully!")
            break
        elif choice.upper() == 'Q':
            sys.exit()
        else:
            print("\n❌ Invalid choice. Please input e, d, or q.")
