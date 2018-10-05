<template>
    <div class="login container-fluid">
        <div class="row header bg-primary">
            <div class="col-6">
                <h1 class="mt-2 text-white">UnPinterested</h1>
            </div>
            <div class="col-6">
                <button @click="loginForm = !loginForm" class="btn btn-secondary mb-3 mt-3">Login</button>
                <button @click="registerForm = !registerForm" class="btn btn-secondary ml-2 mb-3 mt-3">Register</button>
            </div>
        </div>
        <div>
            <form v-if="loginForm" @submit.prevent="loginUser">
                <input type="email" v-model="creds.email" placeholder="email">
                <input type="password" v-model="creds.password" placeholder="password">
                <button type="submit">Login</button>
            </form>
        </div>
        <div>
            <form v-if="registerForm" @submit.prevent="register">
                <input type="text" v-model="newUser.username" placeholder="name">
                <input type="email" v-model="newUser.email" placeholder="email">
                <input type="password" v-model="newUser.password" placeholder="password">
                <button type="submit">Register</button>
            </form>
        </div>

    </div>
</template>

<script>
    export default {
        name: "login",
        mounted() {
            //checks for valid session
            this.$store.dispatch("authenticate");
        },
        data() {
            return {
                loginForm: false,
                registerForm: false,
                creds: {
                    email: "",
                    password: ""
                },
                newUser: {
                    email: "",
                    password: "",
                    username: ""
                }
            };
        },
        methods: {
            register() {
                this.$store.dispatch("register", this.newUser);
            },
            loginUser() {
                this.$store.dispatch("login", this.creds);
            }
        }

    };
</script>