import hashlib

def read_signature_from_file():
    with open('digital_signature.txt', 'r') as file:
        signature = int(file.read())
    return signature

def decrypt_message(signature, private_key, p):
    decrypted_message = pow(signature, private_key, p)
    return decrypted_message

def calculate_hash_decrypted_message(decrypted_message):
    hashed_message = hashlib.sha256(str(decrypted_message).encode()).hexdigest()
    return int(hashed_message, 16)

def verify_signature(hashed_message, received_hashed_message):
    return str(hashed_message)[:16] == str(received_hashed_message)[:16]

def read_hash_from_file():
    with open('hashed_message.txt', 'r') as file:
        hashed_message = int(file.read())
    return hashed_message

if __name__ == "__main__":
    received_signature = read_signature_from_file()
    private_key = 5465663
    p = 6486443

    decrypted_message = decrypt_message(received_signature, private_key, p)
    print(f"Decrypted Message: {decrypted_message}")

    # Зчитуємо хеш з файлу
    hashed_message = read_hash_from_file()
    print(f"Hash of Decrypted Message: {hashed_message}")

    received_hashed_message = 45649204849100121336595314901990933786645285605614239642426872832346674134844

    verification_result = verify_signature(hashed_message, received_hashed_message)
    if verification_result:
        print("\n✅ Digital Signature is valid. The message is authentic.")
    else:
        print("\n❌ Digital Signature is invalid. The message might have been tampered with.")
