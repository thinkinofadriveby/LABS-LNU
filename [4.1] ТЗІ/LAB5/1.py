import random
import math

def is_prime(num):
    if num < 2:
        return False
    for i in range(2, int(math.sqrt(num)) + 1):
        if num % i == 0:
            return False
    return True

def generate_prime():
    while True:
        prime_candidate = random.randint(2**15, 2**16)
        if is_prime(prime_candidate):
            return prime_candidate

def gcd(a, b):
    while b:
        a, b = b, a % b
    return a

def generate_keys():
    p = generate_prime()
    q = generate_prime()

    n = p * q
    phi = (p - 1) * (q - 1)

    while True:
        e = random.randint(2, phi - 1)
        if gcd(e, phi) == 1:
            break

    d = pow(e, -1, phi)
    return (n, e, p, q), (n, d)

def encrypt(message, public_key):
    n, e = public_key[:2]
    encrypted = [pow(ord(char), e, n) for char in message]
    return encrypted

def decrypt(encrypted_message, private_key):
    n, d = private_key[:2]
    decrypted = [chr(pow(char, d, n)) for char in encrypted_message]
    return ''.join(decrypted)

def save_to_file(filename, data):
    with open(filename, 'w') as file:
        file.write(data)

def read_from_file(filename):
    with open(filename, 'r') as file:
        return file.read()

(public_key, private_key) = generate_keys()

n, e, p, q = public_key
print(f"p: {p}")
print(f"q: {q}")
print(f"e: {e}")
print(f"d: {private_key[1]}")

message = read_from_file('plaintext.txt')

encrypted_message = encrypt(message, public_key)

save_to_file('encrypted_message.txt', ' '.join(map(str, encrypted_message)))

decrypted_message = decrypt(encrypted_message, private_key)

print("\nDecrypted Message:", decrypted_message)
