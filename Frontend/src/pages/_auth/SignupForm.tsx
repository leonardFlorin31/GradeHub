import { useState } from "react";
import { Link } from "react-router-dom";
import { z } from "zod";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form";
import { AlertCircle, Loader2 } from "lucide-react";
import { useAuthContext } from "@/AuthContext";
import { Alert, AlertDescription } from "@/components/ui/alert";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";

const SignUpValidation = z
  .object({
    name: z.string().min(2, "Name must be at least 2 characters"),
    email: z.string().email("Please enter a valid email address"),
    password: z.string().min(6, "Password must be at least 6 characters"),
    confirmPassword: z.string(),
    role: z.enum(["teacher", "student"]),
    classId: z.string().optional(),
    className: z.string().optional(),
  })
  .refine((data) => data.password === data.confirmPassword, {
    message: "Passwords don't match",
    path: ["confirmPassword"],
  })
  .refine(
    (data) => data.role === "student" || (data.classId && data.className),
    {
      message: "Class ID and Class Name are required for teachers",
      path: ["classId"],
    }
  );

const SignupForm = () => {
  const { register, isLoading, error, clearError } = useAuthContext();
  const [showPassword, setShowPassword] = useState(false);
  const [showConfirmPassword, setShowConfirmPassword] = useState(false);

  const form = useForm<z.infer<typeof SignUpValidation>>({
    resolver: zodResolver(SignUpValidation),
    defaultValues: {
      name: "",
      email: "",
      password: "",
      confirmPassword: "",
      role: "student",
      classId: "",
      className: "",
    },
  });

  const onSubmit = async (values: z.infer<typeof SignUpValidation>) => {
    clearError();

    const userData = {
      name: values.name,
      email: values.email,
      password: values.password,
      role: values.role,
      classId: values.classId,
      className: values.className,
    };

    const { success } = await register(userData);
    if (!success) return;
  };

  return (
    <div className="flex flex-1 flex-col bg-light-blue items-center justify-center p-4 sm:p-6 lg:p-8">
      <div className="w-full max-w-md space-y-8">
        <div className="text-center text-rich-black">
          <h1 className="text-2xl font-bold tracking-tight">
            Create an account
          </h1>
          <p className="mt-2 text-md">Get started with GradeHub</p>
        </div>

        {/* Error message from AuthContext */}
        {error && (
          <Alert
            variant="destructive"
            className="border-choco-cosmos bg-choco-cosmos/10"
          >
            <AlertCircle className="h-4 w-4 text-choco-cosmos" />
            <AlertDescription className="text-choco-cosmos">
              {error}
            </AlertDescription>
          </Alert>
        )}

        <Form {...form}>
          <form
            onSubmit={form.handleSubmit(onSubmit)}
            className="mt-8 space-y-6"
            noValidate
          >
            <div className="space-y-4 text-rich-black">
              {/* Name Field */}
              <FormField
                control={form.control}
                name="name"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Full Name</FormLabel>
                    <FormControl>
                      <Input
                        type="text"
                        autoComplete="name"
                        disabled={isLoading}
                        placeholder="Your name"
                        className="focus:ring-bice-blue focus:border-bice-blue border-rich-black/20"
                        {...field}
                      />
                    </FormControl>
                    <FormMessage className="text-red-600" />
                  </FormItem>
                )}
              />

              {/* Email Field */}
              <FormField
                control={form.control}
                name="email"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Email address</FormLabel>
                    <FormControl>
                      <Input
                        type="email"
                        autoComplete="email"
                        disabled={isLoading}
                        placeholder="you@example.com"
                        className="focus:ring-bice-blue focus:border-bice-blue border-rich-black/20"
                        {...field}
                      />
                    </FormControl>
                    <FormMessage className="text-red-600" />
                  </FormItem>
                )}
              />

              {/* Password Field */}
              <FormField
                control={form.control}
                name="password"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Password</FormLabel>
                    <div className="relative">
                      <FormControl>
                        <Input
                          type={showPassword ? "text" : "password"}
                          autoComplete="new-password"
                          disabled={isLoading}
                          placeholder="••••••••"
                          className="focus:ring-bice-blue focus:border-bice-blue border-rich-black/20"
                          {...field}
                        />
                      </FormControl>
                      <Button
                        type="button"
                        variant="ghost"
                        size="sm"
                        className="absolute right-0 top-0 h-full px-3 py-2 hover:bg-transparent"
                        onClick={() => setShowPassword(!showPassword)}
                      >
                        {showPassword ? "Hide" : "Show"}
                      </Button>
                    </div>
                    <FormMessage className="text-red-600" />
                  </FormItem>
                )}
              />

              {/* Confirm Password Field */}
              <FormField
                control={form.control}
                name="confirmPassword"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Confirm Password</FormLabel>
                    <div className="relative">
                      <FormControl>
                        <Input
                          type={showConfirmPassword ? "text" : "password"}
                          autoComplete="new-password"
                          disabled={isLoading}
                          placeholder="••••••••"
                          className="focus:ring-bice-blue focus:border-bice-blue border-rich-black/20"
                          {...field}
                        />
                      </FormControl>
                      <Button
                        type="button"
                        variant="ghost"
                        size="sm"
                        className="absolute right-0 top-0 h-full px-3 py-2 hover:bg-transparent"
                        onClick={() =>
                          setShowConfirmPassword(!showConfirmPassword)
                        }
                      >
                        {showConfirmPassword ? "Hide" : "Show"}
                      </Button>
                    </div>
                    <FormMessage className="text-red-600" />
                  </FormItem>
                )}
              />

              {/* Role Selection */}
              <FormField
                control={form.control}
                name="role"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Are you a teacher or student?</FormLabel>
                    <div className="flex space-x-4">
                      <Button
                        type="button"
                        variant={
                          field.value === "teacher" ? "default" : "outline"
                        }
                        className={`flex-1 ${
                          field.value === "teacher"
                            ? "bg-bice-blue text-white"
                            : "border-bice-blue text-bice-blue"
                        }`}
                        onClick={() => field.onChange("teacher")}
                      >
                        Teacher
                      </Button>
                      <Button
                        type="button"
                        variant={
                          field.value === "student" ? "default" : "outline"
                        }
                        className={`flex-1 ${
                          field.value === "student"
                            ? "bg-bice-blue text-white"
                            : "border-bice-blue text-bice-blue"
                        }`}
                        onClick={() => field.onChange("student")}
                      >
                        Student
                      </Button>
                    </div>
                    <FormMessage className="text-red-600" />
                  </FormItem>
                )}
              />
            </div>

            {form.watch("role") === "teacher" && (
              <>
                {/* Class ID */}
                <FormField
                  control={form.control}
                  name="classId"
                  render={({ field }) => (
                    <FormItem>
                      <FormLabel className="text-black">
                        Class ID (e.g. MATH101)
                      </FormLabel>
                      <FormControl>
                        <Input
                          type="text"
                          placeholder="Unique backend ID"
                          disabled={isLoading}
                          className="focus:ring-bice-blue focus:border-bice-blue border-rich-black/20 text-black"
                          {...field}
                        />
                      </FormControl>
                      <FormMessage className="text-red-600" />
                    </FormItem>
                  )}
                />

                {/* Class Name */}
                <FormField
                  control={form.control}
                  name="className"
                  render={({ field }) => (
                    <FormItem>
                      <FormLabel className="text-black">
                        Class Name (e.g. Mathematics 101)
                      </FormLabel>
                      <FormControl>
                        <Input
                          type="text"
                          placeholder="Display name"
                          disabled={isLoading}
                          className="focus:ring-bice-blue focus:border-bice-blue border-rich-black/20 text-black"
                          {...field}
                        />
                      </FormControl>
                      <FormMessage className="text-red-600" />
                    </FormItem>
                  )}
                />
              </>
            )}

            {/* Submit Button */}
            <div className="flex justify-center">
              <Button
                type="submit"
                disabled={isLoading}
                className="w-1/2 bg-bice-blue hover:bg-blue-950 text-white"
              >
                {isLoading ? (
                  <>
                    <Loader2 className="mr-2 h-4 w-4 animate-spin" />
                    Signing up...
                  </>
                ) : (
                  "Sign up"
                )}
              </Button>
            </div>
          </form>
        </Form>

        {/* Already have an account link */}
        <div className="text-center text-sm text-rich-black/70">
          Already have an account?{" "}
          <Button
            asChild
            variant="link"
            size="sm"
            className="px-0 text-bice-blue hover:text-bice-blue/80"
          >
            <Link to="/sign-in" onClick={clearError}>
              Sign in
            </Link>
          </Button>
        </div>
      </div>
    </div>
  );
};

export default SignupForm;
