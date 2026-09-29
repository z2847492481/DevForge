<script setup lang="ts">
import { reactive, ref } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { ElMessage } from 'element-plus'
import type { FormInstance, FormRules } from 'element-plus'
import { User, Lock, Key, Message, UserFilled } from '@element-plus/icons-vue'
import { useAuthStore } from '@/stores/auth'
import type { RegisterPayload } from '@/api/auth'

const route = useRoute()
const router = useRouter()
const auth = useAuthStore()

const formRef = ref<FormInstance>()
const form = reactive({
  fullName: '',
  username: '',
  email: '',
  password: '',
  confirmPassword: ''
})

const rules: FormRules = {
  fullName: [{ max: 255, message: 'Full name must be at most 255 characters', trigger: 'blur' }],
  username: [
    { required: true, message: 'Please choose a username', trigger: 'blur' },
    { min: 2, max: 255, message: 'Username must be 2–255 characters', trigger: 'blur' }
  ],
  email: [
    { required: true, message: 'Please enter your email', trigger: 'blur' },
    { type: 'email', message: 'Please enter a valid email address', trigger: 'blur' }
  ],
  password: [
    { required: true, message: 'Please set a password', trigger: 'blur' },
    { min: 6, max: 128, message: 'Password must be 6–128 characters', trigger: 'blur' }
  ],
  confirmPassword: [
    { required: true, message: 'Please confirm your password', trigger: 'blur' },
    {
      validator: (_: unknown, value: string, callback: (err?: Error) => void) => {
        if (value !== form.password) callback(new Error('Passwords do not match'))
        else callback()
      },
      trigger: 'blur'
    }
  ]
}

async function handleSubmit() {
  if (!formRef.value) return
  const valid = await formRef.value.validate().catch(() => false)
  if (!valid) return

  const payload: RegisterPayload = {
    username: form.username.trim(),
    email: form.email.trim(),
    password: form.password,
    fullName: form.fullName.trim() || undefined
  }

  try {
    await auth.register(payload)
    ElMessage.success(`Welcome to DevForge, ${auth.displayName}!`)
    const redirect = typeof route.query.redirect === 'string' ? route.query.redirect : undefined
    router.push(redirect && redirect.startsWith('/') ? redirect : { name: 'home' })
  } catch (err) {
    ElMessage.error(err instanceof Error ? err.message : 'Registration failed')
  }
}
</script>

<template>
  <div class="register-page">
    <!-- Brand panel -->
    <aside class="register-brand">
      <div class="brand-inner">
        <div class="brand-logo">
          <el-icon :size="30"><Key /></el-icon>
          <span>DevForge</span>
        </div>
        <h1 class="brand-title">One account, full traceability.</h1>
        <p class="brand-subtitle">
          Manage requirements, applications, and developers — and see how every piece connects.
        </p>
      </div>
    </aside>

    <!-- Form panel -->
    <main class="register-form-panel">
      <div class="register-card">
        <h2 class="register-title">Create your account</h2>
        <p class="register-muted">Join DevForge to start building.</p>

        <el-form
          ref="formRef"
          :model="form"
          :rules="rules"
          label-position="top"
          size="large"
          @keyup.enter="handleSubmit"
        >
          <el-form-item label="Full name (optional)" prop="fullName">
            <el-input v-model="form.fullName" :prefix-icon="UserFilled" placeholder="Jane Doe" />
          </el-form-item>

          <el-form-item label="Username" prop="username">
            <el-input v-model="form.username" :prefix-icon="User" placeholder="jane" autocomplete="off" />
          </el-form-item>

          <el-form-item label="Email" prop="email">
            <el-input v-model="form.email" :prefix-icon="Message" placeholder="jane@example.com" autocomplete="off" />
          </el-form-item>

          <el-form-item label="Password" prop="password">
            <el-input
              v-model="form.password"
              type="password"
              :prefix-icon="Lock"
              placeholder="6+ characters"
              show-password
              autocomplete="new-password"
            />
          </el-form-item>

          <el-form-item label="Confirm password" prop="confirmPassword">
            <el-input
              v-model="form.confirmPassword"
              type="password"
              :prefix-icon="Lock"
              placeholder="Repeat your password"
              show-password
              autocomplete="new-password"
            />
          </el-form-item>

          <el-button type="primary" class="register-submit" :loading="auth.loading" @click="handleSubmit">
            Create account
          </el-button>
        </el-form>

        <div class="register-foot">
          Already have an account?<RouterLink to="/login" class="register-link">Sign in</RouterLink>
        </div>
      </div>
    </main>
  </div>
</template>

<style scoped>
.register-page {
  min-height: 100vh;
  display: grid;
  grid-template-columns: 1.1fr 1fr;
}

.register-brand {
  background: radial-gradient(120% 120% at 15% 0%, #22315c 0%, var(--df-bg) 65%);
  display: flex;
  align-items: center;
  padding: 64px;
}

.brand-inner {
  max-width: 460px;
}

.brand-logo {
  display: flex;
  align-items: center;
  gap: 12px;
  font-size: 22px;
  font-weight: 700;
  color: var(--df-text);
  letter-spacing: 0.5px;
}

.brand-logo .el-icon {
  display: grid;
  place-items: center;
  width: 44px;
  height: 44px;
  border-radius: 12px;
  background: linear-gradient(135deg, var(--df-primary), #8a63f5);
  color: #fff;
}

.brand-title {
  margin: 40px 0 16px;
  font-size: 38px;
  line-height: 1.2;
  font-weight: 700;
}

.brand-subtitle {
  margin: 0;
  font-size: 17px;
  line-height: 1.7;
  color: var(--df-text-muted);
}

.register-form-panel {
  display: grid;
  place-items: center;
  padding: 48px;
  overflow-y: auto;
}

.register-card {
  width: 100%;
  max-width: 380px;
}

.register-title {
  margin: 0 0 6px;
  font-size: 26px;
  font-weight: 700;
}

.register-muted {
  margin: 0 0 24px;
  color: var(--df-text-muted);
}

.register-submit {
  width: 100%;
  margin-top: 8px;
  font-weight: 600;
  letter-spacing: 0.3px;
}

.register-foot {
  margin-top: 20px;
  text-align: center;
  color: var(--df-text-muted);
  font-size: 14px;
}

.register-link {
  margin-left: 6px;
  font-weight: 600;
  color: var(--df-primary);
}

:deep(.el-input__wrapper) {
  background: var(--df-bg-soft);
  box-shadow: 0 0 0 1px var(--df-border) inset;
  border-radius: 8px;
}
:deep(.el-input__inner) {
  color: var(--df-text);
  caret-color: var(--df-text);
}
:deep(.el-form-item__label) {
  color: var(--df-text);
}
:deep(.el-input__inner:-webkit-autofill),
:deep(.el-input__inner:-webkit-autofill:hover),
:deep(.el-input__inner:-webkit-autofill:focus) {
  -webkit-text-fill-color: var(--df-text);
  -webkit-box-shadow: 0 0 0 1000px var(--df-bg-soft) inset;
  caret-color: var(--df-text);
  background-color: transparent;
  transition: background-color 9999s ease-in-out 0s;
}

@media (max-width: 900px) {
  .register-page {
    grid-template-columns: 1fr;
  }
  .register-brand {
    display: none;
  }
}
</style>